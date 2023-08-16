using MySpot.Core.ValueObjects;

namespace MySpot.Core.Entities;

public sealed class CleaningReservations : Reservation
{
    private CleaningReservations()
    {
    }
    
    public CleaningReservations(ReservationId id, ParkingSpotId parkingSpotId, 
        ParkingSpotCapacity fullCapacity, Date date)
        : base(id, parkingSpotId, fullCapacity, date)
    {
    }
}