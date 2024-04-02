namespace SolarWatch.Service;

public interface ICoordinateProvider
{
    string GetLatLon(string city);
}