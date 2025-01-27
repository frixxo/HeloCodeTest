using System.Collections;
using System.Configuration;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using PokemonCatcher.Infrastructure.Persistence.DatabaseRepresentations;
using PokemonCatcher.Model;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace PokemonCatcher.Controllers;

[Route("api/v1/pokemons")]
public sealed class PokemonController : ControllerBase
{
    private static string[] validPokemonTypes = new string[]
{
    "normal", "fighting", "flying", "poison", "ground", "rock", "bug", "ghost",
    "steel", "fire", "water", "grass", "electric", "psychic", "ice", "dragon",
    "dark", "fairy", "stellar"
};
    private readonly ApplicationDbContext _context;
    private readonly PokeDexApiContext _pokeDexContext;
    public PokemonController(ApplicationDbContext context,PokeDexApiContext pokedexContext)
    {
        _context = context;
        _pokeDexContext = pokedexContext;
    }

    /// <summary>
    /// Endpoint used to catch a Pokémon, only needs one of name or PokeDexId.
    /// </summary>
    /// <param name="TrainerId">The id of the Pokémon trainer.</param>
    /// <param name="nameOrId">The name or id of the pokeDexId of the pokemon to catch.</param>
    /// <returns></returns>
    [HttpPost("catch-pokemon/{nameOrId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CatchPokemonAsync([FromRoute] String? nameOrId, [FromQuery] int trainerId)
    {
        //CheckpokemonAsync returns null for null ínputs
        var result = await _pokeDexContext.checkPokemonAsync(nameOrId.ToLower());

        if (result == null) return (NotFound($"Pokemon not in PokeDex: {nameOrId}"));

        PokemonTrainerDB? trainer=null;
        
        var trainerquery = _context.PokemonTrainers.AsQueryable();
        trainerquery = trainerquery.Where(trainer => trainer.Id == trainerId);
        var tmp = await trainerquery.ToListAsync();
        trainer = tmp.FirstOrDefault();
        
        if(trainer == null) return (NotFound($"Trainer not found"));

        var query = _context.Pokemons.AsQueryable();
        query = query.Where(pokemon => pokemon.PokeDexId == result.id);
        var ps = await query.ToListAsync();

        //Pokemon not in our database
        if (ps.Count == 0)
        {
            PokemonDB pokemonDB = new PokemonDB { Name = result.name.ToLower(), PokeDexId = result.id };
            await _context.Pokemons.AddAsync(pokemonDB);
            _context.SaveChanges();

            ps.Add(pokemonDB);

            foreach (PokemonTypeEntry e in result.types)
            {
                await _context.PokemonTypes.AddAsync(new PokemonTypeDB { TypeName = e.type.name.ToLower(), PokemonId = pokemonDB.Id });
            }
            foreach (AbilityEntry e in result.abilities)
            {
                await _context.PokemonAbilities.AddAsync(new AbilityDB { Name = e.ability.name.ToLower(), PokemonId = pokemonDB.Id, Slot = e.slot });
            }
        }

        var p = ps[0]; //PokedexId unique so can only be one

        await _context.caughtPokemons.AddAsync(new CaughtPokemonDB { CatchTime = DateTime.Now, PokemonName = p.Name, PokemonId = p.Id, TrainerId = trainer.Id });
        _context.SaveChanges();

        return (Ok($"Caught Pokémon: {p.Name}"));
    }


    /// <summary>
    /// Endpoint to retrieve the caught Pokémons
    /// </summary>
    /// <param name="Types">Types of pokemon to filter on (Optional).</param>
    /// <param name="trainerId">The trainer Id whose pokemon we want to se.</param>
    /// <returns></returns>
    [HttpGet("get-caught-pokemons/{trainerId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetCaughtPokemonsAsync([FromRoute] int trainerId, [FromQuery] List<string?> Types)
    {
        
        var query = _context.caughtPokemons.AsQueryable();
        query = query.Where(pokemon => pokemon.TrainerId == trainerId);

        if (!Types.IsNullOrEmpty())
        {
            Types.ForEach(t => t.ToLower());
            var faults = getFaultyTypes(Types);
            if (!faults.IsNullOrEmpty()) return BadRequest(generateInvalidTypeMessage(faults));
            query = query.Where(pokemon => _context.PokemonTypes
                .Where(type => type.PokemonId == pokemon.Id)
                .Select(type => type.TypeName)
                .Any(typeName => Types.Contains(typeName)));
        }

        var CaughtPokemon = await query.ToListAsync();
        
        // Return an OK response with the list of Pokémons
        return (Ok(CaughtPokemon));
    }

    private string generateInvalidTypeMessage(List<string> faults)
    {
        StringBuilder sb = new StringBuilder();
        foreach(string fault in faults)
        {
            sb.Append(fault + ", ");
        }
        if (faults.Count == 1) sb.Append("is not a valid pokemon type.");
        else sb.Append("is not valid pokemon types.");

        return sb.ToString();
    }

    private List<string> getFaultyTypes(List<string?> types) {
        List<string> faults = new List<string>();
        foreach(string type in types)
        {
            if (!validPokemonTypes.Contains(type)) faults.Add(type);
        }
        return faults;
    }

    /// <summary>
    /// Endpoint to retrieve all Pokémons in database
    /// </summary>
    /// <param type="Types">Types of pokemon to filter on (Optional).</param>
    /// <returns></returns>
    [HttpGet("get-all-pokemons")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetPokemonsAsync([FromQuery] List<string?> Types)
    {
        var query = _context.Pokemons.AsQueryable();

        if (!Types.IsNullOrEmpty())
        {
            Types.ForEach(t => t.ToLower());
            var faults = getFaultyTypes(Types);
            if (!faults.IsNullOrEmpty()) return BadRequest(generateInvalidTypeMessage(faults));
            query = query.Where(pokemon => _context.PokemonTypes
                .Where(type => type.PokemonId == pokemon.Id)
                .Select(type => type.TypeName)
                .Any(typeName => Types.Contains(typeName)));
        }

        var pokemon = await query.ToListAsync();

        return (Ok(pokemon));
    }


    /// <summary>
    /// Endpoint to retrieve all Pokémon trainers in database
    /// </summary>
    /// <returns></returns>
    [HttpGet("get-all-trainers")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTrainersAsync()
    {
        var trainers = await _context.PokemonTrainers.ToListAsync();

        // Return an OK response with the list of Pokémons
        return (Ok(trainers));
    }

    /// <summary>
    /// Endpoint to retrieve all Pokémon types
    /// </summary>
    /// <returns></returns>
    [HttpGet("get-types")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> getTypes()
    {
        return Ok(validPokemonTypes);
    }

    /// <summary>
    /// Endpoint to insert new Pokémon trainer into database
    /// </summary>
    /// <param name="trainer">The name of the Pokémon Trainer.</param>
    /// <returns></returns>
    [HttpPut("create-new-trainer")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTrainersAsync([FromBody] PokemonTrainerDB trainer)
    {
        //var trainer = new PokemonTrainerDB { Level = level, Name = name };
        _context.PokemonTrainers.Add(trainer);

        await _context.SaveChangesAsync();

        // Return an OK response with the list of Pokémons
        return (Ok("trainer added with Id: "+trainer.Id));
    }

    /// <summary>
    /// Endpoint to retrieve the more info on a pokemon, enter name or id.
    /// </summary>
    /// <returns></returns>
    [HttpGet("get-pokemon-info/{nameOrId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetPokemonInfosAsync([FromRoute] string nameOrId)
    {
        var query = _context.Pokemons.AsQueryable();
        if (int.TryParse(nameOrId, out int pokemonId)){
            query = query.Where(pokemon => pokemon.Id == pokemonId);
        }
        else
        {
            query = query.Where(pokemon => pokemon.Name.Equals(nameOrId.ToLower()));
        }
            
        var pokemons = query.ToListAsync();
        if (pokemons.Result.Count == 0) return NotFound("Pokemon not in database");
        if (pokemons.Result.Count > 1) return StatusCode(500, "Multiple Pokemons with same name found");
        var pokemon = pokemons.Result[0];

        var typeQuery = _context.PokemonTypes.AsQueryable();
        typeQuery = typeQuery.Where(type => type.PokemonId == pokemon.Id);
        var types = await typeQuery.ToListAsync();

        var abilityQuery = _context.PokemonAbilities.AsQueryable();
        abilityQuery = abilityQuery.Where(type => type.PokemonId == pokemon.Id);
        var abilities = await abilityQuery.ToListAsync();

        // Return an OK response with the list of Pokémons
        return Ok(new Pokemon(pokemon, types, abilities));
    }

    /// <summary>
    /// Endpoint used to release a catched Pokémon
    /// </summary>
    /// <param id="PokemonId">The id of the specific caught Pokémon instance to release.</param>
    /// <param id="transferTrainerId">The id of the owner of the pokemon.</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpPost("release-pokemon/{trainerId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ReleasePokemonAsync([FromRoute] int trainerId, [FromQuery] int Id)
    {
       
        var query = _context.caughtPokemons.AsQueryable();
        query = query.Where(pokemon => pokemon.Id == Id);
        var ps = await query.ToListAsync();
        if (ps.Count == 0) return NotFound("Pokemon instance not found");
        var p = ps[0];
        
        var query2 = _context.Pokemons.AsQueryable();
        query2 = query2.Where(pokemon => pokemon.Id == p.PokemonId);
        var ps2 = await query2.ToListAsync();
        if (ps2.Count == 0) return NotFound("Pokemon species not found");
        var p2 = ps2[0];

        if (p.TrainerId != trainerId) return Unauthorized("pokemon not owned by that trainer");

        var trainerQuery = _context.PokemonTrainers.AsQueryable();
        trainerQuery = trainerQuery.Where(trainer => trainer.Id == trainerId);
        var ts = await trainerQuery.ToListAsync();
        if (ts.Count == 0) return NotFound("Trainer not found");
        var trainer = ts[0];

        _context.caughtPokemons.Remove(p);
        await _context.SaveChangesAsync();

        return Ok($"Released Pokémon of species {p2.Name} owned by {trainer.Name}");
    }

}
