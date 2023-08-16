using MySpot.Core.ValueObjects;

namespace MySpot.Core.Exceptions;

public sealed class ParkingSpotCapacityExceededException : MySpotException
{
    public ParkingSpotId ParkingSpotId { get; }

    public ParkingSpotCapacityExceededException(ParkingSpotId parkingSpotId) 
        : base($"Parking spot with ID: {parkingSpotId} exceeds its reservation capacity.")
    {
        ParkingSpotId = parkingSpotId;
    }
}