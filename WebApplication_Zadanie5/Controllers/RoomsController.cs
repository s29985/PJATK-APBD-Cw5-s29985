using Microsoft.AspNetCore.Mvc;
using System.Linq;

[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAll([FromQuery] int? minCapacity, [FromQuery] bool? hasProjector, [FromQuery] bool? activeOnly)
    {
        var rooms = AppData.Rooms.AsQueryable();

        if (minCapacity.HasValue)
            rooms = rooms.Where(r => r.Capacity >= minCapacity.Value);

        if (hasProjector.HasValue)
            rooms = rooms.Where(r => r.HasProjector == hasProjector.Value);

        if (activeOnly == true)
            rooms = rooms.Where(r => r.IsActive);

        return Ok(rooms.ToList());
    }

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var room = AppData.Rooms.FirstOrDefault(r => r.Id == id);
        if (room == null) return NotFound();

        return Ok(room);
    }

    [HttpGet("building/{buildingCode}")]
    public IActionResult GetByBuilding(string buildingCode)
    {
        var rooms = AppData.Rooms
            .Where(r => r.BuildingCode.ToLower() == buildingCode.ToLower())
            .ToList();

        return Ok(rooms);
    }

    [HttpPost]
    public IActionResult Create(Room room)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        room.Id = AppData.Rooms.Any() ? AppData.Rooms.Max(r => r.Id) + 1 : 1;
        AppData.Rooms.Add(room);

        return CreatedAtAction(nameof(Get), new { id = room.Id }, room);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, Room updatedRoom)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var room = AppData.Rooms.FirstOrDefault(r => r.Id == id);
        if (room == null) return NotFound();

        room.Name = updatedRoom.Name;
        room.BuildingCode = updatedRoom.BuildingCode;
        room.Floor = updatedRoom.Floor;
        room.Capacity = updatedRoom.Capacity;
        room.HasProjector = updatedRoom.HasProjector;
        room.IsActive = updatedRoom.IsActive;

        return Ok(room);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var room = AppData.Rooms.FirstOrDefault(r => r.Id == id);
        if (room == null) return NotFound();

        if (AppData.Reservations.Any(r => r.RoomId == id))
            return Conflict("Room has reservations");

        AppData.Rooms.Remove(room);

        return NoContent();
    }
}
