using System;
using HealthyCountry.Models;

namespace HealthyCountry.RTC.Models;

public class WebSocketEventModel
{
    public Guid Id { get; set; }

    public DateTime Date { get; set; }

    public string EventName { get; set; }

    public string RoomId { get; set; }

    public string UserId { get; set; }

    public int ActiveUserCount { get; set; }

    public UserRoles Role { get; set; }
}