using Microsoft.Extensions.Options;
using PokemonCatcher.Infrastructure.PokeDexAPI;
using System.Text.Json;

public class PokeDexApiContext : ControllerBase
{
    private readonly HttpClient _httpClient;
    private readonly PokeDexAPISettings _settings;

    public PokeDexApiContext(HttpClient httpClient, IOptions<PokeDexAPISettings> options)
    {
        _httpClient = httpClient;
        _settings = options.Value;
        _httpClient.BaseAddress = new Uri(_settings.BaseUrl);
    }
    public async Task<T?> QueryApiAsync<T>(string endpoint)
    {
        try
        {
            // Make the API request to the given endpoint
            var response = await _httpClient.GetStringAsync(endpoint);

            var data = JsonSerializer.Deserialize<T>(response);

            if (data == null)
            {
                return default; 
            }

            return data;
        }
        catch (HttpRequestException httpEx)
        {
            Console.WriteLine($"HTTP error: {httpEx.Message}");
            return default;
        }
        catch (JsonException jsonEx)
        {
            Console.WriteLine($"JSON error: {jsonEx.Message}");
            return default;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return default;
        }
    }

    public async Task<PokeDexEntry?> checkPokemonAsync(string? name)
    {
        if (name == null) return null;
        return await QueryApiAsync<PokeDexEntry>("pokemon/"+name);
    }
}

public class PokeDexEntry
{
    public string name { set; get; }
    public int id { set; get; }
    public PokemonTypeEntry[] types { set; get; }

    public AbilityEntry[] abilities { set; get; }
}
public class PokemonTypeEntry
{
    public PokemonTypeDetail type { set; get; }
}
public class PokemonTypeDetail
{
    public string name { set; get; }
}
public class AbilityEntry
{
    public int slot { get; set; }
    public AbilityEntryDetail ability { set; get; }
}
public class AbilityEntryDetail
{
    public string name { set; get; }
}

