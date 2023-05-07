using Microsoft.EntityFrameworkCore;
using NLog.Web;
using ParkingHereApi.Entities;
using ParkingHereApi.Middleware;
using ParkingHereApi.Seeder;
using ParkingHereApi.Services;
using System.Reflection;
using System.Runtime.CompilerServices;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(LogLevel.Trace);
builder.Host.UseNLog();

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<ParkingDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ParkingHereDbConnection")));
builder.Services.AddScoped<ParkingHereSeeder>();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddScoped<IParkingService, ParkingService>();
builder.Services.AddScoped<ErrorHandlingMiddleware>();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.

var scope = app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetRequiredService<ParkingHereSeeder>();

seeder.Seed();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ParkingHereApi");
});

app.UseAuthorization();

app.MapControllers();

app.Run();
