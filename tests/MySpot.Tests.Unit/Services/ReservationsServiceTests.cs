﻿using MySpot.Api.Commands;
using MySpot.Api.Entities;
using MySpot.Api.Models;
using MySpot.Api.Services;
using MySpot.Api.ValueObjects;
using Shouldly;
using Xunit;

namespace MySpot.Tests.Unit.Services;

public class ReservationsServiceTests
{
    #region Arrange

    private readonly ReservationsService _reservationService;
    private static readonly Clock Clock = new();

    private readonly List<WeeklyParkingSpot> _weeklyParkingSpots = new()
    {
        new WeeklyParkingSpot(Guid.Parse("00000000-0000-0000-0000-000000000001"), new Week(Clock.Current()), "P1"),
        new WeeklyParkingSpot(Guid.Parse("00000000-0000-0000-0000-000000000002"), new Week(Clock.Current()), "P2"),
        new WeeklyParkingSpot(Guid.Parse("00000000-0000-0000-0000-000000000003"), new Week(Clock.Current()), "P3"),
        new WeeklyParkingSpot(Guid.Parse("00000000-0000-0000-0000-000000000004"), new Week(Clock.Current()), "P4"),
        new WeeklyParkingSpot(Guid.Parse("00000000-0000-0000-0000-000000000005"), new Week(Clock.Current()), "P5"),
    };
    

    public ReservationsServiceTests()
    {
        _reservationService = new ReservationsService(_weeklyParkingSpots);
    }

    #endregion

    [Fact]
    public void given_reservation_for_not_taken_date_add_reservation_should_succeed()
    {
        // ARRANGE
        var parkingSpot = _weeklyParkingSpots.First(); 
        var command = new CreateReservation(
            parkingSpot.Id,
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