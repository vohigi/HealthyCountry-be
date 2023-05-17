using HealthyCountry.Models;

namespace HealthyCountry.RTC.Models;

public class UserModel
{
    public string Id { get; set; }

    public UserRoles Role { get; set; }
        
    public string ConnectionId { get; set; }
}