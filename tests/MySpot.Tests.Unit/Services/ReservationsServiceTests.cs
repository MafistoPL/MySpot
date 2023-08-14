using MySpot.Application.Commands;
using MySpot.Application.Services;
using MySpot.Core.Repositories;
using MySpot.Infrastructure.DAL.Repositories;
using MySpot.Tests.Unit.Shared;
using Shouldly;
using Xunit;

namespace MySpot.Tests.Unit.Services;

public class ReservationsServiceTests
{
    #region Arrange

    private readonly ReservationsService _reservationService;
    private readonly IClock _clock;
    private readonly IWeeklyParkingSpotRepository _weeklyParkingSpotRepository;

    public ReservationsServiceTests()
    {
        _clock = new TestClock();
        _weeklyParkingSpotRepository = new InMemoryWeeklyParkingSpotRepository(_clock);
        _reservationService = new ReservationsService(_weeklyParkingSpotRepository, _clock);
    }

    #endregion

    [Fact]
    public async Task given_reservation_for_not_taken_date_add_reservation_should_succeed()
    {
        // ARRANGE
        var parkingSpot = (await _weeklyParkingSpotRepository.GetAllAsync()).First(); 
        var command = new CreateReservation(
            parkingSpot.Id,
            "John Doe",
            "XYZ123",
            _clock.Current().AddMinutes(5)
        );
        
        // ACT
        var reservationId = await _reservationService.CreateAsync(command);

        // ASSERT
        reservationId.ShouldNotBeNull();
        reservationId.Value.ShouldBe(command.ReservationId);
        
    }
}