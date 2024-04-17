using SolarWatch.Model;

namespace SolarWatch.Service.Repository;

public interface ISunsetSunriseRepository
{
    IEnumerable<SunsetSunrise> GetAll();
    SunsetSunrise? GetByCity(City city);

    void Add(SunsetSunrise sunsetSunrise);
    void Delete(SunsetSunrise sunsetSunrise);
    void Update(SunsetSunrise sunsetSunrise);
}