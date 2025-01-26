using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace PokemonCatcher.Infrastructure.Persistence.DatabaseRepresentations
{
    public class AbilityDB
    {
        public int? Id { get; set; }
        public int? PokemonId { get; set; }
        public int Slot { get; set; }
        public string? Name { set; get; }
    }
}