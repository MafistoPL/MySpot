using MySpot.Core.Exceptions;
using MySpot.Core.ValueObjects;

namespace MySpot.Core.Entities;

public class WeeklyParkingSpot
{
    public const int MaxParkingSpotCapacity = 2;
    
    private readonly HashSet<Reservation> _reservations = new();
    
    public ParkingSpotId Id { get; private set; }
    public Week Week { get; private set; }
    public ParkingSpotName Name { get; private set; }
    public ParkingSpotCapacity ParkingSpotCapacity { get; private set; }
    public IEnumerable<Reservation> Reservations => _reservations;

    private WeeklyParkingSpot(ParkingSpotId id, Week week, ParkingSpotName name, ParkingSpotCapacity parkingSpotCapacity)
    {
        Id = id;
        Week = week;
        Name = name;
        ParkingSpotCapacity = parkingSpotCapacity;
    }

    public static WeeklyParkingSpot Create(ParkingSpotId id, Week week, ParkingSpotName name)
        => new(id, week, name, MaxParkingSpotCapacity);

    internal void AddReservation(Reservation reservation, Date now)
    {
        var isInvalidDate = reservation.Date < Week.From ||
                            reservation.Date > Week.To ||
                            reservation.Date < now;
        if (isInvalidDate)
        {
            throw new InvalidReservationDateException(reservation.Date.Value.Date);
        }

        var dateCapacity = _reservations
            .Where(x => x.Date == reservation.Date)
            .Sum(x => x.ParkingSpotCapacity);

        if (dateCapacity + reservation.ParkingSpotCapacity > ParkingSpotCapacity)
        {
            throw new ParkingSpotCapacityExceededException(Id);
        }

        _reservations.Add(reservation);
    }

    public void RemoveReservation(Guid reservationId)
    {
        var reservationToRemove = _reservations.FirstOrDefault(r => r.Id.Value == reservationId);

        if (reservationToRemove == null)
        {
            throw new ReservationNotFoundException(reservationId);
        }
        
        _reservations.Remove(reservationToRemove);
    }
    
    public void RemoveReservations(IEnumerable<Reservation> reservations)
        => _reservations.RemoveWhere(x => reservations.Any(r => r.Id == x.Id));
}