using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SolarWatch.Contracts;
using SolarWatch.Data;
using SolarWatch.Model;
using SolarWatch.Service.Authentication;

namespace IntegrationTest;

public class SolarWatchWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _dbName = Guid.NewGuid().ToString();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var solarWatchDbContextDescriptor =
                services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<SolarWatchContext>));

            services.Remove(solarWatchDbContextDescriptor);

            services.AddDbContext<SolarWatchContext>(options => options.UseInMemoryDatabase(_dbName));
            

            using var scope = services.BuildServiceProvider().CreateScope();

            var solarContext = scope.ServiceProvider.GetRequiredService<SolarWatchContext>();
            solarContext.Database.EnsureDeleted();
            solarContext.Database.EnsureCreated();
        });
    }
    
    
    
    [Collection("IntegrationTests")] 
    public class MyControllerIntegrationTest
    {
        private readonly SolarWatchWebApplicationFactory _app;
        private readonly HttpClient _client;
        
        public MyControllerIntegrationTest()
        {
            _app = new SolarWatchWebApplicationFactory();
            _client = _app.CreateClient();
        }
        
        [Fact]
        public async Task TestRegister()
        {
            var registerResponse = await _client.PostAsJsonAsync("Auth/Register", new {Email = "test@mail.com", UserName="test", Password = "test1111"});
            registerResponse.EnsureSuccessStatusCode();
            
            Assert.Equal(HttpStatusCode.Created, registerResponse.StatusCode);
        }
        
        [Fact]
        public async Task TestRegisterSameEmail()
        {
            var registerResponse1 = await _client.PostAsJsonAsync("Auth/Register", new {Email = "test@mail.com", UserName="test", Password = "test1111"});
            registerResponse1.EnsureSuccessStatusCode();
            var registerResponse2 = await _client.PostAsJsonAsync("Auth/Register", new {Email = "test@mail.com", UserName="testasd", Password = "test1111"});
            
            
            Assert.Equal(HttpStatusCode.BadRequest, registerResponse2.StatusCode);
        }
        
        [Fact]
        public async Task TestRegisterWrongPassword()
        {
            var registerResponse = await _client.PostAsJsonAsync("Auth/Register", new {Email = "test@mail.com", UserName="test", Password = "test"});
            
            Assert.Equal(HttpStatusCode.BadRequest, registerResponse.StatusCode);
        }
        
        [Fact]
        public async Task TestSuccessfulLogin()
        {
            var loginResponse = await _client.PostAsJsonAsync("Auth/Login", new {Email = "admin@admin.com", Password = "admin123"});
            loginResponse.EnsureSuccessStatusCode();
            var loginResult = await loginResponse.Content.ReadFromJsonAsync<AuthResponse>();
            
            Assert.Equal("admin@admin.com", loginResult.Email);
        }
        
        [Fact]
        public async Task TestWrongLogin()
        {
            var loginResponse = await _client.PostAsJsonAsync("Auth/Login", new {Email = "admin@admin", Password = "admin123"});

            Assert.Equal(HttpStatusCode.BadRequest, loginResponse.StatusCode);

            var loginResult = await loginResponse.Content.ReadFromJsonAsync<AuthResult>();

            Assert.False(loginResult.Success); 
        }
        
        [Fact]
        public async Task TestGetSunsetSunrise()
        {
            var loginResponse = await _client.PostAsJsonAsync("Auth/Login", new {Email = "admin@admin.com", Password = "admin123"});
            loginResponse.EnsureSuccessStatusCode();
            var loginResult = await loginResponse.Content.ReadFromJsonAsync<AuthResponse>();
            
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", loginResult.Token);
            
            var response = await _client.GetAsync("SolarWatch/GetSunsetSunrise?city=Budapest");
            
            response.EnsureSuccessStatusCode();
            
            var data = await response.Content.ReadFromJsonAsync<SunsetSunrise>();
            Assert.Equal("Budapest", data.City.Name);
        }
        
        [Fact]
        public async Task TestGetCities()
        {
            var loginResponse = await _client.PostAsJsonAsync("Auth/Login", new {Email = "admin@admin.com", Password = "admin123"});
            loginResponse.EnsureSuccessStatusCode();
            var loginResult = await loginResponse.Content.ReadFromJsonAsync<AuthResponse>();
            
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", loginResult.Token);
            
            var response = await _client.GetAsync("SolarWatch/GetCities");
            
            response.EnsureSuccessStatusCode();
            
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        
        [Fact]
        public async Task TestCreateCityWithAdmin()
        {
            var loginResponse = await _client.PostAsJsonAsync("Auth/Login", new {Email = "admin@admin.com", Password = "admin123"});
            loginResponse.EnsureSuccessStatusCode();
            var loginResult = await loginResponse.Content.ReadFromJsonAsync<AuthResponse>();
            
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", loginResult.Token);
            
            var response = await _client.PostAsJsonAsync("SolarWatch/AddCity", new {Name = "test", Longitude = "12", Latitude = "1", State = "test", Country = "test"});
            
            response.EnsureSuccessStatusCode();
            
            Assert.Equal(1, response.Content.ReadFromJsonAsync<City>().Result.Latitude);
        }
        
        [Fact]
        public async Task TestAdminEndpointWithUser()
        {
            var registerResponse1 = await _client.PostAsJsonAsync("Auth/Register", new {Email = "test@mail.com", UserName="test", Password = "test1111"});
            registerResponse1.EnsureSuccessStatusCode();
            var loginResponse = await _client.PostAsJsonAsync("Auth/Login", new {Email = "test@mail.com", Password = "test1111"});
            loginResponse.EnsureSuccessStatusCode();
            var loginResult = await loginResponse.Content.ReadFromJsonAsync<AuthResponse>();
            
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", loginResult.Token);
            
            var response = await _client.PostAsJsonAsync("SolarWatch/AddCity", new {Name = "test", Longitude = "12", Latitude = "1", State = "test", Country = "test"}); 
            
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

    }


}