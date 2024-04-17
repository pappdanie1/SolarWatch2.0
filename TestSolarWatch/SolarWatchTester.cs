using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SolarWatch.Controllers;
using SolarWatch.Data;
using SolarWatch.Service;
using SolarWatch.Service.Repository;

namespace TestSolarWatch;

public class SolarWatchTester
{ 
    private Mock<ILogger<SolarWatchController>> _loggerMock;
    private Mock<ICoordinateProvider> _coordinateProviderMock;
    private Mock<ISunDataProvider> _sunDataProviderMock;
    private Mock<IJsonProcessor> _jsonProcessorMock;
    private SolarWatchController _controller;
    private Mock<ICityRepository> _cityRepositoryMock;
    private Mock<ISunsetSunriseRepository> _sunsetSunriseRepositoryMock;
    private Mock<SolarWatchContext> _dbContextMock;

    [SetUp]
    public void SetUp()
    {
        _loggerMock = new Mock<ILogger<SolarWatchController>>();
        _coordinateProviderMock = new Mock<ICoordinateProvider>();
        _sunDataProviderMock = new Mock<ISunDataProvider>();
        _jsonProcessorMock = new Mock<IJsonProcessor>();
        
        
        _cityRepositoryMock = new Mock<ICityRepository>();
        _sunsetSunriseRepositoryMock = new Mock<ISunsetSunriseRepository>();

        _controller = new SolarWatchController(_loggerMock.Object, _coordinateProviderMock.Object, _jsonProcessorMock.Object, _sunDataProviderMock.Object, _sunsetSunriseRepositoryMock.Object, _cityRepositoryMock.Object);
    }
    
    [Test]
    public async Task Get_InvalidCity_ReturnsBadRequest()
    {
        var city = "NY";

        var result = await _controller.Get(city);
        
        Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
    }

    [Test]
    public async Task Get_ExceptionThrown_ReturnsNotFound()
    {
        var city = "New York";
        _coordinateProviderMock.Setup(cp => cp.GetCity(city)).Throws(new Exception());

        var result = await _controller.Get(city);

        Assert.IsInstanceOf<NotFoundObjectResult>(result.Result);
    }
    
    [Test]
    public async Task Get_EmptyCity_ReturnsBadRequest()
    {
        string city = ""; 

        var result = await _controller.Get(city);

        Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
    }

    [Test]
    public async Task Get_NullCity_ReturnsBadRequest()
    {
        string city = null; 

        var result = await _controller.Get(city);

        Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
    }
    
    
    
}