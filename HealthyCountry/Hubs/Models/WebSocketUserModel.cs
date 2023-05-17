using System.Text.Json.Serialization;
using HealthyCountry.Models;

namespace HealthyCountry.Hubs.Models;

/// <summary>
/// Authorized user data
/// </summary>
public class WebSocketUserModel
{

    [JsonPropertyName("id")]
    public string RelativeId { get; set; }
    
    public UserRoles Role { get; set; }
    public string ConnectionId { get; set; }
}