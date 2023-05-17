namespace HealthyCountry.RTC.Models;

public class EventDetailsModel
{
    public string PatientId { get; set; }
    
    public string ResourceId { get; set; }
    
    public string EventId { get; set; }
    
    public string PatientCreator { get; set; }
    
    public string ResourceUserId { get; set; }
    
    public string DateBegin { get; set; }
    
    public string DateEnd { get; set; }
    
    public string EventState { get; set; }
    
    public string EventPlaceId { get; set; }
    
    public string IsOnlineEvent { get; set; }
}