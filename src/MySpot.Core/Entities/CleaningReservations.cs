using MySpot.Core.ValueObjects;

namespace MySpot.Core.Entities;

public sealed class CleaningReservations : Reservation
{
    private CleaningReservations()
    {
    }
    
    public CleaningReservations(ReservationId id, ParkingSpotId parkingSpotId, Date date)
        : base(id, parkingSpotId, date)
    {
    }
}