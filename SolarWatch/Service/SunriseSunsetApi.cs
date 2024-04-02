using System.Net;

namespace SolarWatch.Service;

public class SunriseSunsetApi : ISunDataProvider
{
    private readonly ILogger<SunriseSunsetApi> _logger;

    public SunriseSunsetApi(ILogger<SunriseSunsetApi> logger)
    {
        _logger = logger;
    }


    public string GetSunriseSunset(double lat, double lon)
    {
        var url = $"https://api.sunrise-sunset.org/json?lat={lat}&lng={lon}";
        
        using var client = new WebClient();
        
        _logger.LogInformation("Calling Sunrise Sunset API with url: {url}", url);
        return client.DownloadString(url);
    }
}