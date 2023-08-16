namespace MySpot.Application.Commands;

public record ReserveParkingSpotForVehicle(
    Guid ParkingSpotId,
    string EmployeeName,
    string LicensePlate,
    int ParkingSpotCapacity,
    DateTime Date)
{
    public Guid ReservationId { get; } = Guid.NewGuid();
}