using MySpot.Api.Models;

namespace MySpot.Api.Services;

public class ReservationsService
{
    private static readonly List<string> ParkingSpotNames = new List<string>
    {
        "P1", "P2", "P3", "P4", "P5"
    };
    
    private static int _id = 1;
    private static readonly List<Reservation> Reservations = new();

    public Reservation Get(int id) 
        => Reservations.SingleOrDefault(x => x.Id == id);

    public IEnumerable<Reservation> GetAll() 
        => Reservations;

    public int? Create(Reservation reservation)
    {
        var now = DateTime.UtcNow.Date;
        var pastDays = now.DayOfWeek is DayOfWeek.Sunday ? 7 : (int) now.DayOfWeek;
        var remainingDays = 7 - pastDays;
        
        if (!ParkingSpotNames.Contains(reservation.ParkingSpotName))
        {
            return default;
        }

        if (!(reservation.Date.Date >= now && reservation.Date.Date <= now.AddDays(remainingDays)))
        {
            return default;
        }
        
        if (string.IsNullOrWhiteSpace(reservation.LicensePlate))
        {
            return default;
        }

        var reservationAlreadyExists = Reservations.Any(r =>
            r.ParkingSpotName == reservation.ParkingSpotName && r.Date == reservation.Date);
        if (reservationAlreadyExists)
        {
            return default;
        }
        
        reservation.Id = _id++;
        Reservations.Add(reservation);

        return reservation.Id;
    }

    public bool Update(Reservation reservation)
    {
        var existingReservation = Reservations.SingleOrDefault(x => x.Id == reservation.Id);
        if (existingReservation is null)
        {
            return false;
        }

        if (existingReservation.Date <= DateTime.UtcNow)
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(reservation.LicensePlate))
        {
            return false;
        }
        
        existingReservation.LicensePlate = reservation.LicensePlate;

        return true;
    }

    public bool Delete(int id)
    {
        var existingReservation = Reservations.SingleOrDefault(x => x.Id == id);
        if (existingReservation is null)
        {
            return false;
        }
        Reservations.Remove(existingReservation);
        
        return true;
    }
}