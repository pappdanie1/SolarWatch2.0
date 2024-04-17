namespace SolarWatch.Model;

public class SunsetSunrise
{
    public Guid Id { get; init; }
    public string Sunrise { get; set; }
    public string Sunset { get; set; }
    public City City { get; set; }
}