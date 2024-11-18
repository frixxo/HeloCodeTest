namespace PokemonCatcher.Controllers;

[Route("api/v1/pokemons")]
public sealed class PokemonController : ControllerBase
{
    /// <summary>
    /// Endpoint used to catch a Pokémon
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpPost("catch-pokemon")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public Task<IActionResult> CatchPokemonAsync()
    {
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Endpoint to retrieve the caught Pokémons
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpGet("get-caught-pokemons")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public Task<IActionResult> GetPokemonsAsync()
    {
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Endpoint used to release a catched Pokémon
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpPost("release-pokemon")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public Task<IActionResult> ReleasePokemonAsync()
    {
        throw new NotImplementedException();
    }
    
    
}
