namespace MySpot.Api.Exceptions;

public sealed class EmptyLicensePlateException : MySpotException
{
    public EmptyLicensePlateException() : base("License plate is empty")
    {
        
    }
}