namespace SolarWatch.Service;

public interface ISunDataProvider
{
    string GetSunriseSunset(double lat, double lon);
}