using PokemonCatcher.Infrastructure.Persistence.Context.DatabaseRepresentations;

namespace PokemonCatcher.Model
{
    public class Pokemon
    {
        public int Id { get; set; }
        public int PokeDexId { get; set; }
        public string Name { get; set; }
        public List<string>? Types { get; set; }
        public List<Ability>? Abilities { get; set; }

        public Pokemon(PokemonDB? pokemon, List<PokemonTypeDB>? types, List<AbilityDB>? abilities)
        {
            Id = pokemon.Id;
            PokeDexId = pokemon.PokeDexId;
            Name = pokemon.Name;

            Types = new List<string>();
            foreach (PokemonTypeDB db in types) Types.Add(db.TypeName);

            Abilities = new List<Ability>();
            foreach (AbilityDB db in abilities) Abilities.Add(new Ability(db));
        }
    }
}
