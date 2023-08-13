using MySpot.Application;
using MySpot.Core;
using MySpot.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddInfrastructure()
    .AddApplication()
    .AddCore()
    .AddControllers();

var app = builder.Build();
app.MapControllers();

app.Run();
