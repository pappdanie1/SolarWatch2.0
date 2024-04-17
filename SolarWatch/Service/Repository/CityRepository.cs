using SolarWatch.Data;
using SolarWatch.Model;

namespace SolarWatch.Service.Repository;

public class CityRepository : ICityRepository
{
    private readonly SolarWatchContext _solarWatchContext;

    public CityRepository(SolarWatchContext solarWatchContext)
    {
        _solarWatchContext = solarWatchContext;
    }

    public IEnumerable<City> GetAll()
    {
        return _solarWatchContext.Cities.ToList();
    }

    public City? GetByName(string name)
    {
        return _solarWatchContext.Cities.FirstOrDefault(c => c.Name == name);
    }

    public void Add(City city)
    {
        _solarWatchContext.Add(city);
        _solarWatchContext.SaveChanges();
    }

    public void Delete(City city)
    {
        _solarWatchContext.Remove(city);
        _solarWatchContext.SaveChanges();
    }

    public void Update(City city)
    {
        _solarWatchContext.Update(city);
        _solarWatchContext.SaveChanges();
    }
}