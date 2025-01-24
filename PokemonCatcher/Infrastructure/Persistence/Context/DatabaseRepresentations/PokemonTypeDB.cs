using System.ComponentModel.DataAnnotations;

namespace PokemonCatcher.Infrastructure.Persistence.Context.DatabaseRepresentations
{
    public class PokemonTypeDB
    {
        public int? Id { get; set; }
        public int? PokemonId { get; set; }
        public string? TypeName { get; set; }
    }
}