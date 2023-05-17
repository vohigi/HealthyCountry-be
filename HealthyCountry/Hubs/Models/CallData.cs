using System.Text.Json.Serialization;

namespace HealthyCountry.Hubs.Models;

public class CallData
{
    [JsonPropertyName("roomId")]
    public string RoomId { get; set; }

    [JsonPropertyName("signal")]
    public string Signal { get; set; }

    [JsonPropertyName("activeUsersCount")]
    public int? ActiveUsersCount { get; set; }
}