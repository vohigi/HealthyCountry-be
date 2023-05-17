namespace HealthyCountry.Hubs.Models;

public class SaveConferenceEventRequest : BaseConferenceRequest
{
    public string EventName { get; set; }
}