using Microsoft.EntityFrameworkCore;
using SolarWatch.Model;

namespace SolarWatch.Data;

public class SolarWatchContext : DbContext
{
    private readonly IConfiguration _configuration;

    public SolarWatchContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public DbSet<City> Cities { get; set; }
    public DbSet<SunsetSunrise> SunsetSunrises { get; set; }
    
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string connectionString = _configuration.GetConnectionString("DefaultConnection");
        optionsBuilder.UseSqlServer(connectionString, options =>
        {
            options.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
        });
    }
}