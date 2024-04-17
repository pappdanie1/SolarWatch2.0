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

    [HttpGet("GetSolarWatch")]
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
}