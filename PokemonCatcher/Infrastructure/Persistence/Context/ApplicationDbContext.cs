using PokemonCatcher.Infrastructure.Persistence.DatabaseRepresentations;

namespace PokemonCatcher.Infrastructure.Persistence.Context;

public sealed class ApplicationDbContext : DbContext
{
    private readonly IConfiguration? _configuration;
    
    public ApplicationDbContext()
    {
    }
   
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration conf) : base(options)
    {
        _configuration = conf;
    }
    

    // Define your DbSet properties here
    public DbSet<PokemonDB> Pokemons { get; set; }
    public DbSet<PokemonTypeDB> PokemonTypes { get; set; }
    public DbSet<AbilityDB> PokemonAbilities { get; set; }
    public DbSet<PokemonTrainerDB> PokemonTrainers { get; set; }
    public DbSet<CaughtPokemonDB> caughtPokemons { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<CaughtPokemonDB>()
            .HasKey(p => p.Id);

        modelBuilder.Entity<CaughtPokemonDB>()
            .Property(p => p.PokemonId)
            .IsRequired();

        modelBuilder.Entity<CaughtPokemonDB>()
            .Property(p => p.TrainerId)
            .IsRequired();

        modelBuilder.Entity<CaughtPokemonDB>()
           .Property(p => p.CatchTime)
           .IsRequired();


        modelBuilder.Entity<PokemonTrainerDB>()
            .HasKey(p => p.Id);

        modelBuilder.Entity<PokemonTrainerDB>()
            .Property(p => p.Name)
            .IsRequired();

        modelBuilder.Entity<PokemonTrainerDB>()
            .Property(p => p.Level)
            .IsRequired();


        modelBuilder.Entity<PokemonDB>()
            .HasKey(p => p.Id);

        modelBuilder.Entity<PokemonDB>()
            .Property(p => p.PokeDexId)
            .IsRequired();

        //enforce unique pokedexId
        modelBuilder.Entity<PokemonDB>()
           .HasIndex(p => p.PokeDexId)
           .IsUnique(); 

        modelBuilder.Entity<PokemonDB>()
            .Property(p => p.Name)
            .IsRequired();


        modelBuilder.Entity<PokemonTypeDB>()
           .HasKey(p => p.Id);

        modelBuilder.Entity<PokemonTypeDB>()
            .Property(p => p.TypeName)
            .IsRequired();

        modelBuilder.Entity<PokemonTypeDB>()
            .Property(p => p.PokemonId)
            .IsRequired();


        modelBuilder.Entity<AbilityDB>()
           .HasKey(p => p.Id);

        modelBuilder.Entity<AbilityDB>()
            .Property(p => p.Slot)
            .IsRequired();

        modelBuilder.Entity<AbilityDB>()
            .Property(p => p.PokemonId)
            .IsRequired();
        modelBuilder.Entity<AbilityDB>()
            .Property(p => p.Name)
            .IsRequired();
    }
}