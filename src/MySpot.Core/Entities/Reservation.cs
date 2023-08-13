using MySpot.Core.ValueObjects;

namespace MySpot.Core.Entities;

public class Reservation
{
    public ReservationId Id { get; }
    public ParkingSpotId ParkingSpotId { get; }
    public EmployeeName EmployeeName { get; private set; }
    public LicensePlate LicensePlate { get; private set; }
    public Date Date { get; private set; }

    public Reservation(Guid id, Guid parkingSpotId, string employeeName, LicensePlate licensePlate, Date date)
    {
        Id = id;
        ParkingSpotId = parkingSpotId;
        EmployeeName = employeeName;
        LicensePlate = licensePlate;
        Date = date;
    }

    public void ChangeLicensePlate(LicensePlate licensePlate) 
        => LicensePlate = licensePlate;
}