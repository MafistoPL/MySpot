using Microsoft.AspNetCore.Mvc;
using MySpot.Api.Models;
using MySpot.Api.Services;

namespace MySpot.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ReservationsController : ControllerBase
{
    private readonly ReservationsService _reservationsService = new();
    
    [HttpGet("{id:int}")]
    public ActionResult<Reservation> Get(int id)
    {
        var reservation = _reservationsService.Get(id);
        if (reservation is null)
        {
            return NotFound();
        }

        return Ok(reservation);
    }
    
    [HttpGet]
    public ActionResult<IEnumerable<Reservation>> GetAll() => Ok(_reservationsService.GetAll());
    
    [HttpPost]
    public ActionResult Post(Reservation reservation)
    {
        var id = _reservationsService.Create(reservation);
        if (id is null)
        {
            return BadRequest();
        }

        return CreatedAtAction(nameof(Get), new {id}, null);
    }

    [HttpPut("{id:int}")]
    public ActionResult Put(int id, Reservation reservation)
    {
        reservation.Id = id;
        if (_reservationsService.Update(reservation))
        {
            return NoContent();
        }
        
        return NotFound();
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        if (_reservationsService.Delete(id))
        {
            return NoContent();
        }
        
        return NotFound();
    }
}