namespace SolarWatch.Service;

public interface ICoordinateProvider
{
    Task<string> GetLatLon(string city);
}