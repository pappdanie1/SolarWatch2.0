using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SolarWatch.Model;

namespace SolarWatch.Data;

public class SolarWatchContext : IdentityDbContext<IdentityUser, IdentityRole, string>
{

    public SolarWatchContext(DbContextOptions<SolarWatchContext> options) : base(options)
    {
    }
    
    public DbSet<City> Cities { get; set; }
    public DbSet<SunsetSunrise> SunsetSunrises { get; set; }
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}