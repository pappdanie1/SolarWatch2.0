using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using SolarWatch.Model;

namespace SolarWatch.Data;

public class SolarWatchContext : DbContext
{
    public SolarWatchContext(DbContextOptions<SolarWatchContext> options) : base(options)
    {
        var dbCreater = Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
        if (dbCreater != null)
        {
            if (!dbCreater.CanConnect())
            {
                dbCreater.Create();
            }

            if (!dbCreater.HasTables())
            {
                dbCreater.CreateTables();
            }
        }
    }
    public DbSet<City> Cities { get; set; }
    public DbSet<SunsetSunrise> SunsetSunrises { get; set; }
    
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost,1433;Database=SolarWatch;User Id=sa;Password=Dani9123%;Encrypt=false;");
    }
}