using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolarWatch.Model;
using SolarWatch.Service;
using SolarWatch.Service.Repository;

namespace SolarWatch.Controllers;

[ApiController]
[Route("[controller]")]
public class SolarWatchController : ControllerBase
{
    private readonly ILogger<SolarWatchController> _logger;
    private readonly ICoordinateProvider _coordinateProvider;
    private readonly ISunDataProvider _sunDataProvider;
    private readonly IJsonProcessor _jsonProcessor;
    private readonly ICityRepository _cityRepository;
    private readonly ISunsetSunriseRepository _sunsetSunriseRepository;

    public SolarWatchController(ILogger<SolarWatchController> logger, ICoordinateProvider coordinateProvider, IJsonProcessor jsonProcessor, ISunDataProvider sunDataProvider, ISunsetSunriseRepository sunsetSunriseRepository, ICityRepository cityRepository)
    {
        _logger = logger;
        _coordinateProvider = coordinateProvider;
        _jsonProcessor = jsonProcessor;
        _sunDataProvider = sunDataProvider;
        _sunsetSunriseRepository = sunsetSunriseRepository;
        _cityRepository = cityRepository;
    }

    [HttpGet("GetSunsetSunrise"), Authorize(Roles="User, Admin")]
    public async Task<ActionResult<SunsetSunrise>> Get(string city)
    {
        _logger.LogInformation("request sent");
        try
        {
            if (string.IsNullOrEmpty(city) || city.Length < 3)
            {
                return BadRequest("City name must be at least 3 characters long.");
            }
            
            var existingCity = _cityRepository.GetByName(city);
            if (existingCity != null)
            {
                var sunsetSunrise = _sunsetSunriseRepository.GetByCity(existingCity);
                return Ok(sunsetSunrise);
            }
            else
            {
                var cityData = await _coordinateProvider.GetCity(city);
                var processedCity = _jsonProcessor.ProcessCity(cityData);
                _cityRepository.Add(processedCity);
                Console.WriteLine(processedCity);

                var sunData = await _sunDataProvider.GetSunriseSunset(processedCity.Latitude, processedCity.Longitude);
                var sunsetSunrise = _jsonProcessor.ProcessSunriseSunset(sunData, processedCity);
                _sunsetSunriseRepository.Add(sunsetSunrise);
                
                return Ok(sunsetSunrise);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting data");
            return NotFound("Error getting data");
        }
    }

    [HttpGet("GetCities"), Authorize(Roles = "User, Admin")]
    public ActionResult GetAllCities()
    {
        try
        {
            var cities = _cityRepository.GetAll();
            return Ok(cities);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting data");
            return NotFound("Error getting data");
        }
    }

    [HttpPost("AddCity"), Authorize(Roles = "Admin")]
    public ActionResult AddCity(City city)
    {
        try
        {
            city.Id = Guid.NewGuid();
            _cityRepository.Add(city);
            return Ok(city);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error adding data");
            return NotFound("Error adding data");
        }
    }
    
    [HttpPost("AddSunsetSunrise"), Authorize(Roles = "Admin")]
    public ActionResult AddSunsetSunrise(SunsetSunrise sunsetSunrise)
    {
        try
        {
            sunsetSunrise.Id = Guid.NewGuid();
            sunsetSunrise.City.Id = Guid.NewGuid();
            _sunsetSunriseRepository.Add(sunsetSunrise);
            return Ok(sunsetSunrise);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error adding data");
            return NotFound("Error adding data");
        }
    }

    [HttpDelete("DeleteCity"), Authorize(Roles = "Admin")]
    public ActionResult DeleteCity(string name)
    {
        try
        {
            var city = _cityRepository.GetByName(name);
            if (city == null)
            {
                return NotFound("City not found");
            }

            _cityRepository.Delete(city);
            return Ok(city);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error deleting data");
            return NotFound("Error deleting data");
        }
    }
    
    [HttpDelete("DeleteSunsetSunrise"), Authorize(Roles = "Admin")]
    public ActionResult DeleteSunsetSunrise(string name)
    {
        try
        {
            var city = _cityRepository.GetByName(name);
            if (city == null)
            {
                return NotFound("City not found");
            }

            var sunsetSunrise = _sunsetSunriseRepository.GetByCity(city);
            _sunsetSunriseRepository.Delete(sunsetSunrise);
            _cityRepository.Delete(city);
            return Ok(sunsetSunrise);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error deleting data");
            return NotFound("Error deleting data");
        }
    }
    
    [HttpPatch("UpdateCity"), Authorize(Roles = "Admin")]
    public ActionResult UpdateCity(City city)
    {
        try
        {
            var existingCity = _cityRepository.GetByName(city.Name);
            if (existingCity == null)
            {
                return NotFound("City not found");
            }

            existingCity.Name = city.Name;
            existingCity.Latitude = city.Latitude;
            existingCity.Longitude = city.Longitude;
            existingCity.State = city.State;
            existingCity.Country = city.Country;

            _cityRepository.Update(existingCity);
            return Ok(existingCity);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error updating city");
            return NotFound("Error updating city");
        }
    }

    [HttpPatch("UpdateSunsetSunrise"), Authorize(Roles = "Admin")]
    public ActionResult UpdateSunsetSunrise(SunsetSunrise sunsetSunrise)
    {
        try
        {
            var city = _cityRepository.GetByName(sunsetSunrise.City.Name);
            if (city == null)
            {
                return NotFound("SunsetSunrise data not found");
            }
            var existingSunsetSunrise = _sunsetSunriseRepository.GetByCity(city);
            if (existingSunsetSunrise == null)
            {
                return NotFound("SunsetSunrise data not found");
            }

            existingSunsetSunrise.Sunrise = sunsetSunrise.Sunrise;
            existingSunsetSunrise.Sunset = sunsetSunrise.Sunset;

            _sunsetSunriseRepository.Update(existingSunsetSunrise);
            return Ok(existingSunsetSunrise);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error updating sunset sunrise data");
            return NotFound("Error updating sunset sunrise data");
        }
    }
}