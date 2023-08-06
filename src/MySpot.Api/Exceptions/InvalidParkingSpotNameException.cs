namespace MySpot.Api.Exceptions;

public class InvalidParkingSpotNameException : MySpotException
{
    public InvalidParkingSpotNameException() : base("Parking spot name is invalid.")
    {
    }
}