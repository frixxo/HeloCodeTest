using System.ComponentModel.DataAnnotations;

namespace PokemonCatcher.Infrastructure.Persistence.DatabaseRepresentations
{
    public class PokemonDB
    {
        public int Id { get; set; }
        public int PokeDexId { get; set; }
        public required string Name { get; set; }
    }

}