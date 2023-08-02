using System.Net;
using System.Resources;
using Microsoft.AspNetCore.Mvc;
using MySpot.Api.Models;

namespace MySpot.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ReservationsController : ControllerBase
{
    private static readonly List<string> ParkingSpotNames = new List<string>
    {
        "P1", "P2", "P3", "P4", "P5"
    };
    
    private static int _id = 1;
    private static readonly List<Reservation> Reservations = new();

    [HttpGet("{id:int}")]
    public Reservation Get(int id)
    {
        var reservation = Reservations.SingleOrDefault(x => x.Id == id);
        if (reservation is null)
        {
            HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
            return default;
        }

        return reservation;
    }
    
    [HttpGet]
    public IEnumerable<Reservation> GetAll() => Reservations;
    
    [HttpPost]
    public void Post(Reservation reservation)
    {
        if (!ParkingSpotNames.Contains(reservation.ParkingSpotName))
        {
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            return;
        }

        reservation.Date = DateTime.UtcNow.AddDays(1).Date;
        var reservationAlreadyExists = Reservations.Any(r =>
            r.ParkingSpotName == reservation.ParkingSpotName && r.Date == reservation.Date);
        if (reservationAlreadyExists)
        {
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            return;
        }
        
        reservation.Id = _id++;
        Reservations.Add(reservation);
    }
}