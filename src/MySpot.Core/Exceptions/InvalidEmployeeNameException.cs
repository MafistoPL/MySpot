namespace MySpot.Core.Exceptions;

public class InvalidEmployeeNameException : MySpotException
{
    public InvalidEmployeeNameException() : base("Employee name is invalid.")
    {
    }
}