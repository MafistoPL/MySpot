using MySpot.Api.Commands;
using MySpot.Api.Models;
using MySpot.Api.Services;
using Shouldly;
using Xunit;

namespace MySpot.Tests.Unit.Services;

public class ReservationsServiceTests
{
    private readonly ReservationsService _reservationService;

    #region Arrange

    public ReservationsServiceTests()
    {
        _reservationService = new ReservationsService();
    }

    #endregion

    [Fact]
    public void given_reservation_for_not_taken_date_add_reservation_should_succeed()
    {
        // ARRANGE
        var command = new CreateReservation(
            Guid.Parse("00000000-0000-0000-0000-000000000001"),
            "John Doe",
            "XYZ123",
            DateTime.UtcNow.AddMinutes(5)
        );
        
        // ACT
        var reservationId = _reservationService.Create(command);

        // ASSERT
        reservationId.ShouldNotBeNull();
        reservationId.Value.ShouldBe(command.ReservationId);
        
    }
}