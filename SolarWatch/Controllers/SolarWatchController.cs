using Microsoft.AspNetCore.Mvc;
using SolarWatch.Service;

namespace SolarWatch.Controllers;

[ApiController]
[Route("[controller]")]
public class SolarWatchController : ControllerBase
{
    private readonly ILogger<SolarWatchController> _logger;
    private readonly ICoordinateProvider _coordinateProvider;
    private readonly ISunDataProvider _sunDataProvider;
    private readonly IJsonProcessor _jsonProcessor;

    public SolarWatchController(ILogger<SolarWatchController> logger, ICoordinateProvider coordinateProvider, IJsonProcessor jsonProcessor, ISunDataProvider sunDataProvider)
    {
        _logger = logger;
        _coordinateProvider = coordinateProvider;
        _jsonProcessor = jsonProcessor;
        _sunDataProvider = sunDataProvider;
    }

    [HttpGet("GetSolarWatch")]
    public ActionResult<SolarWatchResponse> Get(string city)
    {
        _logger.LogInformation("request sent");
        try
        {
            if (string.IsNullOrEmpty(city) || city.Length < 3)
            {
                return BadRequest("City name must be at least 3 characters long.");
            }
            
            var coordsData = _coordinateProvider.GetLatLon(city);
            var coords = _jsonProcessor.ProcessLatLon(coordsData);
            var lat = coords[0];
            var lon = coords[1];
            
            var sunData = _sunDataProvider.GetSunriseSunset(lat, lon);
            var sunsetSunrise = _jsonProcessor.ProcessSunriseSunset(sunData);
            
            return Ok(sunsetSunrise);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting data");
            return NotFound("Error getting data");
        }
    }
}