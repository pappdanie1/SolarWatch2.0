namespace SolarWatch.Service;

public interface IJsonProcessor
{
    List<double> ProcessLatLon(string data);
    SolarWatchResponse ProcessSunriseSunset(string data);
}