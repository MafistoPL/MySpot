using MySpot.Api.Entities;
using MySpot.Api.Exceptions;
using MySpot.Api.Models;
using MySpot.Api.ValueObjects;
using Shouldly;
using Xunit;

namespace MySpot.Tests.Unit.Entities;

public class WeeklyParkingSpotTests
{
    #region Arrange

    private readonly Date _now;
    private readonly WeeklyParkingSpot _weeklyParkingSpot;

    public WeeklyParkingSpotTests()
    {
        _now = new Date(new DateTime(2022, 08, 10));
        _weeklyParkingSpot = new WeeklyParkingSpot(Guid.NewGuid(), new Week(_now), "P1");
    }

    #endregion

    [Theory]
    [InlineData("2022-08-09")]
    [InlineData("2022-08-17")]
    public void given_invalid_date_add_reservation_should_fail(string dateString)
    {
        // ARRANGE
        var invalidDate = DateTime.Parse(dateString);
        var reservation = new Reservation(Guid.NewGuid(), _weeklyParkingSpot.Id, 
            "John Doe", "XYZ123", new Date(invalidDate));

        // ACT
        var exception = Record.Exception(
            () => _weeklyParkingSpot.AddReservation(reservation, _now));

        // ASSERT
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<InvalidReservationDateException>();
    }

    [Fact]
    public void given_reservation_for_already_existing_date_add_reservation_should_fail()
    {
        // ARRANGE
        var reservationDate = _now.AddDays(1);
        var reservation = new Reservation(Guid.NewGuid(), _weeklyParkingSpot.Id, 
            "John Doe", "XYZ123", reservationDate);
        var nextReservation = new Reservation(Guid.NewGuid(), _weeklyParkingSpot.Id, 
            "John Doe", "XYZ123", reservationDate);
        _weeklyParkingSpot.AddReservation(reservation, _now);
        
        // ACT
        var exception = Record.Exception(
            () => _weeklyParkingSpot.AddReservation(reservation, _now));
        
        // ASSERT
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ParkingSpotAlreadyReservedException>();
    }
}