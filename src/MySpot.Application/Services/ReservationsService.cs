﻿using MySpot.Application.Commands;
using MySpot.Application.DTO;
using MySpot.Core.Entities;
using MySpot.Core.Repositiries;
using MySpot.Core.ValueObjects;

namespace MySpot.Application.Services;

public class ReservationsService : IReservationsService
{
    private readonly IClock _clock;
    private readonly IWeeklyParkingSpotRepository _weeklyParkingSpotRepository;

    public ReservationsService(IWeeklyParkingSpotRepository weeklyParkingSpotRepository, IClock clock)
    {
        _weeklyParkingSpotRepository = weeklyParkingSpotRepository;
        _clock = clock;
    }
    
    public ReservationDto Get(Guid id)
        => GetAllWeekly().SingleOrDefault(x => x.Id == id);

    public IEnumerable<ReservationDto> GetAllWeekly()
        => _weeklyParkingSpotRepository.GetAll().SelectMany(x => x.Reservations)
            .Select(x => new ReservationDto
            {
                Id = x.Id,
                ParkingSpotId = x.ParkingSpotId,
                LicensePlate = x.LicensePlate,
                Date = x.Date.Value.Date
            });

    public Guid? Create(CreateReservation command)
    {
        var weeklyParkingSpot = _weeklyParkingSpotRepository.Get(command.ParkingSpotId);
        if (weeklyParkingSpot is null)
        {
            return default;
        }

        var reservation = new Reservation(
            command.ReservationId,
            command.ParkingSpotId,
            command.EmployeeName,
            command.LicensePlate, 
            new Date(command.Date));
        weeklyParkingSpot.AddReservation(reservation, new Date(_clock.Current()));
        _weeklyParkingSpotRepository.Update(weeklyParkingSpot);

        return reservation.Id;
    }

    public bool Update(ChangeReservationLicensePlate command)
    {
        var weeklyParkingSpot = GetWeeklyParkingSpotByReservation(command.ReservationId);
        if (weeklyParkingSpot is null)
        {
            return false;
        }

        var existingReservation = weeklyParkingSpot.Reservations.SingleOrDefault(
            x => x.Id == command.ReservationId);
        if (existingReservation is null)
        {
            return false;
        }

        if (existingReservation.Date.Value <= _clock.Current())
        {
            return false;
        }
        
        existingReservation.ChangeLicensePlate(command.LicensePlate);
        _weeklyParkingSpotRepository.Update(weeklyParkingSpot);

        return true;
    }

    public bool Delete(DeleteReservation command)
    {
        var weeklyParkingSpot = GetWeeklyParkingSpotByReservation(command.ReservationId);
        if (weeklyParkingSpot is null)
        {
            return false;
        }

        var existingReservation = weeklyParkingSpot.Reservations.SingleOrDefault(
            x => x.Id == command.ReservationId);
        if (existingReservation is null)
        {
            return false;
        }

        weeklyParkingSpot.RemoveReservation(command.ReservationId);
        _weeklyParkingSpotRepository.Update(weeklyParkingSpot);

        return true;
    }

    private WeeklyParkingSpot GetWeeklyParkingSpotByReservation(Guid reservationId)
        => _weeklyParkingSpotRepository.GetAll().SingleOrDefault(
            x => x.Reservations.Any(r => r.Id == reservationId));
}