using Microsoft.EntityFrameworkCore;
using SolarWatch.Data;
using SolarWatch.Service;
using SolarWatch.Service.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<SolarWatchContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers(
    options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);
builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<ISunsetSunriseRepository, SunsetSunriseRepository>();
builder.Services.AddSingleton<ICoordinateProvider, GeoCodingApi>();
builder.Services.AddSingleton<ISunDataProvider, SunriseSunsetApi>();
builder.Services.AddSingleton<IJsonProcessor, JsonProcessor>();
builder.Services.AddDbContext<SolarWatchContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseDeveloperExceptionPage();


app.Run();