

using System;
using System.Collections.Generic;

public static class AppData
{
    public static List<Room> Rooms = new List<Room>
    {
        new Room { Id = 1, Name = "Room A1", BuildingCode = "A", Floor = 1, Capacity = 20, HasProjector = true, IsActive = true },
        new Room { Id = 2, Name = "Room B1", BuildingCode = "B", Floor = 2, Capacity = 30, HasProjector = false, IsActive = true },
        new Room { Id = 3, Name = "Room C1", BuildingCode = "C", Floor = 3, Capacity = 15, HasProjector = true, IsActive = false },
        new Room { Id = 4, Name = "Room A2", BuildingCode = "A", Floor = 2, Capacity = 25, HasProjector = true, IsActive = true }
    };

    public static List<Reservation> Reservations = new List<Reservation>
    {
        new Reservation
        {
            Id = 1,
            RoomId = 1,
            OrganizerName = "Jan Kowalski",
            Topic = "REST API basics",
            Date = DateTime.Parse("2026-05-10"),
            StartTime = TimeSpan.Parse("10:00"),
            EndTime = TimeSpan.Parse("12:00"),
            Status = "confirmed"
        },
        new Reservation
        {
            Id = 2,
            RoomId = 2,
            OrganizerName = "Anna Nowak",
            Topic = "ASP.NET Core",
            Date = DateTime.Parse("2026-05-11"),
            StartTime = TimeSpan.Parse("09:00"),
            EndTime = TimeSpan.Parse("11:00"),
            Status = "planned"
        }
    };
}
