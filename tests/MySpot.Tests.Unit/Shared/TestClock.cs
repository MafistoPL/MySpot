using MySpot.Application.Services;
using MySpot.Core.Abstractions;

namespace MySpot.Tests.Unit.Shared;

public class TestClock : IClock
{
    public DateTime Current()
        => new DateTime(2022, 08, 11);
}