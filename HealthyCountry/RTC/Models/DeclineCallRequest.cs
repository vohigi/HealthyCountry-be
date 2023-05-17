namespace HealthyCountry.RTC.Models;

public class DeclineCallRequest
{
    public string RoomId { get; set; }
    public ReasonCallCancelled Reason { get; set; }
    public UserModel Requester { get; set; }
}