using System.ComponentModel.DataAnnotations;
using PokemonCatcher.Model;

namespace PokemonCatcher.Infrastructure.Persistence.DatabaseRepresentations
{
    public class PokemonTypeDB
    {
        public int? Id { get; set; }
        public int? PokemonId { get; set; }
        public string? TypeName { get; set; }
    }
}