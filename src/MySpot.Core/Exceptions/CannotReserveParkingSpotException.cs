using MySpot.Core.ValueObjects;

namespace MySpot.Core.Exceptions;

public class CannotReserveParkingSpotException : MySpotException
{
    public ParkingSpotId ParkingSpotId { get; }

    public CannotReserveParkingSpotException(ParkingSpotId parkingSpotId) 
        : base($"Cannot reserve parking spot with ID: {parkingSpotId}")
    {
        ParkingSpotId = parkingSpotId;
    }
}