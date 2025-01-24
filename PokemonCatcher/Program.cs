using PokemonCatcher.Controllers;
using PokemonCatcher.Infrastructure.PokeDexAPI;
using PokemonCatcher.Model;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<PokeDexAPISettings>( builder.Configuration.GetSection("PokeDexApi"));
builder.Services.AddHttpClient<PokeDexApiContext>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();

    context.SaveChanges();
}

app.MapControllers();

app.Run();