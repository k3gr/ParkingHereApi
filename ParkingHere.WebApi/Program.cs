using ParkingHere.Application;
using ParkingHere.Domain;
using ParkingHere.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddCore()
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseCors();
app.UseInfrastructure();
app.Run();