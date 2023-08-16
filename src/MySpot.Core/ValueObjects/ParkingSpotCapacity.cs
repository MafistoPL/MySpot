using MySpot.Core.Exceptions;

namespace MySpot.Core.ValueObjects;

public sealed record ParkingSpotCapacity
{
    public int Value { get; set; }

    public ParkingSpotCapacity(int value)
    {
        if (value is < 1 or > 2)
        {
            throw new InvalidCapacityException(value);
        }
        
        Value = value;
    }
    
    public static implicit operator int(ParkingSpotCapacity capacity)
        => capacity.Value;

    public static implicit operator ParkingSpotCapacity(int value)
        => new (value);
}