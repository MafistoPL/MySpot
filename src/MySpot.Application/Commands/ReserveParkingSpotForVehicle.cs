namespace MySpot.Application.Commands;

public record ReserveParkingSpotForVehicle(
    Guid ParkingSpotId,
    string EmployeeName,
    string LicensePlate,
    DateTime Date)
{
    public Guid ReservationId { get; } = Guid.NewGuid();
}