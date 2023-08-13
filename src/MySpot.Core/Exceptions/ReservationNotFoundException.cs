namespace MySpot.Core.Exceptions;

public class ReservationNotFoundException : MySpotException
{
    public ReservationNotFoundException(Guid id) 
        : base($"Reservation with given id: {id} not found")
    {
    }
}