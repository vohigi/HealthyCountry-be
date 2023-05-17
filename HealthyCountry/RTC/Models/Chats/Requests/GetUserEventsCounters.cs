using System.Collections.Generic;

namespace HealthyCountry.RTC.Models.Chats.Requests;

public class GetUserEventsCounters
{
    public string RelativeUserId { get; set; }
    public IEnumerable<string> EventIds { get; set; }
}