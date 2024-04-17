namespace SolarWatch.Service;

public interface ICoordinateProvider
{
    Task<string> GetCity(string city);
}