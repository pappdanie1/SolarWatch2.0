using System.Text.Json;
using SolarWatch.Model;

namespace SolarWatch.Service;

public class JsonProcessor : IJsonProcessor
{
    public City ProcessCity(string data)
    {
        JsonDocument json = JsonDocument.Parse(data);
        JsonElement main = json.RootElement[0];

        var lat = main.GetProperty("lat").GetDouble();
        var lon = main.GetProperty("lon").GetDouble();
        var name = main.GetProperty("name").GetString();
        var stateProperty = main.TryGetProperty("state", out var stateElement);
        var state = stateProperty ? stateElement.GetString() : null;
        var country = main.GetProperty("country").GetString();
        var id = Guid.NewGuid();
        return new City { Id = id, Country = country, Name = name, Latitude = lat, Longitude = lon, State = state};
    }

    public SunsetSunrise ProcessSunriseSunset(string data, City city)
    {
        JsonDocument json = JsonDocument.Parse(data);
        JsonElement results = json.RootElement.GetProperty("results");

        SunsetSunrise sunsetSunrise = new SunsetSunrise
        {
            Id = Guid.NewGuid(),
            Sunrise = results.GetProperty("sunrise").GetString(),
            Sunset = results.GetProperty("sunset").GetString(),
            City = city
        };
        return sunsetSunrise;
    }
    
}