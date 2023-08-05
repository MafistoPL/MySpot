namespace MySpot.Api.Commands;

public record CreateReservation(
    Guid ParkingSpotId,
    string EmployeeName,
    string LicensePlate,
    DateTime Date)
{
    public Guid ReservationId { get; } = Guid.NewGuid();
}