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
}