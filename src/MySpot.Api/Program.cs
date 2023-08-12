using MySpot.Api.Entities;
using MySpot.Api.Repositiries;
using MySpot.Api.Services;
using MySpot.Api.ValueObjects;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddSingleton<IClock, Clock>()
    .AddSingleton<IWeeklyParkingSpotRepository, InMemoryWeeklyParkingSpotRepository>()
    .AddSingleton<ReservationsService>()
    .AddControllers();

var app = builder.Build();
app.MapControllers();
app.Run();
