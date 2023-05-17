using Newtonsoft.Json;

namespace HealthyCountry.RTC.Models;

public class CallPatientRequest
{
    public string RoomId { get; set; }
    
    public string PatientId { get; set; }
    
    [System.Text.Json.Serialization.JsonIgnore, JsonIgnore]
    public UserModel User { get; set; }
}