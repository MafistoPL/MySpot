namespace MySpot.Core.Exceptions;

public class InvalidParkingSpotNameException : MySpotException
{
    public InvalidParkingSpotNameException() : base("Parking spot name is invalid.")
    {
    }
}