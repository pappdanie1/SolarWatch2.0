using SolarWatch.Model;

namespace SolarWatch.Service;

public interface IJsonProcessor
{
    City ProcessCity(string data);
    SunsetSunrise ProcessSunriseSunset(string data, City city);
}