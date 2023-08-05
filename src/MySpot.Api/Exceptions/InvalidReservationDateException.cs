namespace MySpot.Api.Exceptions;

public sealed class InvalidReservationDateException : MySpotException
{
    public DateTime Date { get; }

    public InvalidReservationDateException(DateTime date)
        : base($"Reservation date: {date:d} is invalid.")
    {
        Date = date;
    }
}