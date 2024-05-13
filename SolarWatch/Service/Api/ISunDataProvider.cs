namespace SolarWatch.Service;

public interface ISunDataProvider
{
    Task<string> GetSunriseSunset(double lat, double lon);
}