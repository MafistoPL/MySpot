﻿using MySpot.Api.Exceptions;
using MySpot.Api.Models;
using MySpot.Api.ValueObjects;

namespace MySpot.Api.Entities;

public class WeeklyParkingSpot
{
    private readonly HashSet<Reservation> _reservations = new();
    
    public ParkingSpotId Id { get; }
    public Week Week { get; }
    public ParkingSpotName Name { get; }
    public IEnumerable<Reservation> Reservations => _reservations;

    public WeeklyParkingSpot(ParkingSpotId id, Week week, ParkingSpotName name)
    {
        Id = id;
        Week = week;
        Name = name;
    }

    public void AddReservation(Reservation reservation, Date now)
    {
        var isInvalidDate = reservation.Date < Week.From ||
                            reservation.Date > Week.To ||
                            reservation.Date < now;
        if (isInvalidDate)
        {
            throw new InvalidReservationDateException(reservation.Date.Value.Date);
        }

        var reservationAlreadyExists = Reservations.Any(x =>
            x.Date == reservation.Date);
        if (reservationAlreadyExists)
        {
            throw new ParkingSpotAlreadyReservedException(Name, reservation.Date.Value.Date);
        }

        _reservations.Add(reservation);
    }

    public void RemoveReservation(Guid reservationId)
    {
        var reservationToRemove = _reservations.FirstOrDefault(r => r.Id == reservationId);

        if (reservationToRemove == null)
        {
            throw new ReservationNotFoundException(reservationId);
        }
        
        _reservations.Remove(reservationToRemove);
    }
}