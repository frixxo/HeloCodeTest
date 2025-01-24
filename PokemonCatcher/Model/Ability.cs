using PokemonCatcher.Infrastructure.Persistence.Context.DatabaseRepresentations;

namespace PokemonCatcher.Model
{
    public class Ability
    {
        public int slot { get; set; }
        public string? name { get; set; }

        public Ability(AbilityDB adb)
        {
            this.name = adb.Name;
            this.slot = adb.Slot;
        }
    }
}
