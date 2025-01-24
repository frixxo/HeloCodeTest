namespace PokemonCatcher.Infrastructure.Persistence.Context.DatabaseRepresentations
{
    public class CaughtPokemonDB
    {
        public int Id { get; set; }
        public int PokemonId { get; set; }
        public string? PokemonName { get; set; }
        public int TrainerId { get; set; }
        public DateTime CatchTime { get; set; }
    }
}
