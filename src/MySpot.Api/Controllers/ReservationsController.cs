using System.Resources;
using Microsoft.AspNetCore.Mvc;
using MySpot.Api.Models;

namespace MySpot.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ReservationsController : ControllerBase
{
    private int _id = 1;
    private readonly List<Reservation> _reservations = new();

    private readonly List<string> _parkingSpotNames = new List<string>
    {
        "P1", "P2", "P3", "P4", "P5"
    };
    
    [HttpGet]
    public void Get()
    {
        
    }
    
    [HttpPost]
    public void Post(Reservation reservation)
    {
        if (!_parkingSpotNames.Contains(reservation.ParkingSpotName))
        {
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            return;
        }

        reservation.Date = DateTime.UtcNow.AddDays(1).Date;
        var reservationAlreadyExists = _reservations.Any(r =>
            r.ParkingSpotName == reservation.ParkingSpotName && r.Date == reservation.Date);
        if (reservationAlreadyExists)
        {
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            return;
        }
        
        reservation.Id = _id++;
        _reservations.Add(reservation);
    }
}