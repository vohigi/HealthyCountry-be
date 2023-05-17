using System.Collections.Generic;

namespace HealthyCountry.RTC.Models;

public class IceServerModel
{
    public IList<string> Urls { get; set; }
    
    public string Username { get; set; }
    
    public string Credential { get; set; }

}