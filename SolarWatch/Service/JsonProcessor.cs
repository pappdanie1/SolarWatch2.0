using System.Text.Json;

namespace SolarWatch.Service;

public class JsonProcessor : IJsonProcessor
{
    public List<double> ProcessLatLon(string data)
    {
        var coords = new List<double>();
        JsonDocument json = JsonDocument.Parse(data);
        JsonElement main = json.RootElement[0];

        var lat = main.GetProperty("lat").GetDouble();
        var lon = main.GetProperty("lon").GetDouble();
        coords.Add(lat);
        coords.Add(lon);
        return coords;
    }

    public SolarWatchResponse ProcessSunriseSunset(string data)
    {
        JsonDocument json = JsonDocument.Parse(data);
        JsonElement results = json.RootElement.GetProperty("results");

        SolarWatchResponse solarWatchResponse = new SolarWatchResponse
        {
            Sunrise = results.GetProperty("sunrise").GetString(),
            Sunset = results.GetProperty("sunset").GetString()
        };
        return solarWatchResponse;
    }
    
}