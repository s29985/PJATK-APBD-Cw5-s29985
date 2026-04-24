using Microsoft.AspNetCore.Mvc;
using System.Linq;

[ApiController]
[Route("api/[controller]")]
public class ReservationsController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAll([FromQuery] DateTime? date, [FromQuery] string? status, [FromQuery] int? roomId)
    {
        var reservations = AppData.Reservations.AsQueryable();

        if (date.HasValue)
            reservations = reservations.Where(r => r.Date.Date == date.Value.Date);

        if (!string.IsNullOrEmpty(status))
            reservations = reservations.Where(r => r.Status.ToLower() == status.ToLower());

        if (roomId.HasValue)
            reservations = reservations.Where(r => r.RoomId == roomId.Value);

        return Ok(reservations.ToList());
    }

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var res = AppData.Reservations.FirstOrDefault(r => r.Id == id);
        if (res == null) return NotFound();

        return Ok(res);
    }

    [HttpPost]
    public IActionResult Create(Reservation reservation)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var room = AppData.Rooms.FirstOrDefault(r => r.Id == reservation.RoomId);

        if (room == null)
            return BadRequest("Room does not exist");

        if (!room.IsActive)
            return BadRequest("Room is not active");

        bool conflict = AppData.Reservations.Any(r =>
            r.RoomId == reservation.RoomId &&
            r.Date.Date == reservation.Date.Date &&
            r.StartTime < reservation.EndTime &&
            reservation.StartTime < r.EndTime
        );

        if (conflict)
            return Conflict("Time conflict with existing reservation");

        reservation.Id = AppData.Reservations.Any()
            ? AppData.Reservations.Max(r => r.Id) + 1
            : 1;

        AppData.Reservations.Add(reservation);

        return CreatedAtAction(nameof(Get), new { id = reservation.Id }, reservation);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, Reservation updated)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var res = AppData.Reservations.FirstOrDefault(r => r.Id == id);
        if (res == null) return NotFound();

        res.RoomId = updated.RoomId;
        res.OrganizerName = updated.OrganizerName;
        res.Topic = updated.Topic;
        res.Date = updated.Date;
        res.StartTime = updated.StartTime;
        res.EndTime = updated.EndTime;
        res.Status = updated.Status;

        return Ok(res);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var res = AppData.Reservations.FirstOrDefault(r => r.Id == id);
        if (res == null) return NotFound();

        AppData.Reservations.Remove(res);

        return NoContent();
    }
}
