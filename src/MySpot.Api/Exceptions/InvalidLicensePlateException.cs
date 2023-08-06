namespace MySpot.Api.Exceptions;

public sealed class InvalidLicensePlateException : MySpotException
{
    public string LicensePlate { get; }

    public InvalidLicensePlateException(string licensePlate) 
        : base($"License plate: {licensePlate} is invalid")
    {
        LicensePlate = licensePlate;
    }
}