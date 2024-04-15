using System.Net;

namespace SolarWatch.Service;

public class SunriseSunsetApi : ISunDataProvider
{
    private readonly ILogger<SunriseSunsetApi> _logger;

    public SunriseSunsetApi(ILogger<SunriseSunsetApi> logger)
    {
        _logger = logger;
    }


    public async Task<string> GetSunriseSunset(double lat, double lon)
    {
        var timeZoneId = "Europe/Budapest"; 
        var url = $"https://api.sunrise-sunset.org/json?lat={lat}&lng={lon}&tzid={timeZoneId}";
        
        using var client = new HttpClient();
        
        _logger.LogInformation("Calling Sunrise Sunset API with url: {url}", url);
        var response = await client.GetAsync(url);
        return await response.Content.ReadAsStringAsync();
    }
}