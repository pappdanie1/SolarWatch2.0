using SolarWatch.Data;
using SolarWatch.Model;

namespace SolarWatch.Service.Repository;

public class SunsetSunriseRepository : ISunsetSunriseRepository
{
    private readonly SolarWatchContext _solarWatchContext;

    public SunsetSunriseRepository(SolarWatchContext solarWatchContext)
    {
        _solarWatchContext = solarWatchContext;
    }

    public IEnumerable<SunsetSunrise> GetAll()
    {
        return _solarWatchContext.SunsetSunrises.ToList();
    }

    public SunsetSunrise? GetByCity(City city)
    {
        return _solarWatchContext.SunsetSunrises.FirstOrDefault(s => s.City.Id == city.Id);
    }

    public void Add(SunsetSunrise sunsetSunrise)
    {
        _solarWatchContext.Add(sunsetSunrise);
        _solarWatchContext.SaveChanges();
    }

    public void Delete(SunsetSunrise sunsetSunrise)
    {
        _solarWatchContext.Remove(sunsetSunrise);
        _solarWatchContext.SaveChanges();
    }

    public void Update(SunsetSunrise sunsetSunrise)
    {
        _solarWatchContext.Update(sunsetSunrise);
        _solarWatchContext.SaveChanges();
    }
}