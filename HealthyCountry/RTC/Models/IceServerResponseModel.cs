using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace HealthyCountry.RTC.Models;

public class IceServerResponseModel
{
    [JsonPropertyName("urls")]
    public IList<string> Urls { get; set; }

    [JsonPropertyName("username")]
    public string Username { get; set; }

    [JsonPropertyName("credential")]
    public string Credential { get; set; }
}