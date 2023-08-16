namespace MySpot.Core.Exceptions;

public class InvalidCapacityException : MySpotException
{
    public int Capacity { get; }

    public InvalidCapacityException(int capacity) 
        : base($"Capacity {capacity} is invalid.")
    {
        Capacity = capacity;
    }
}