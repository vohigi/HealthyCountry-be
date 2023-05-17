using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace HealthyCountry.RTC.Models;

public class WebRtcConfigModel
{
    [JsonPropertyName("iceTransportPolicy")]
    public string IceTransportPolicy { get; set; } = "relay";

    [JsonPropertyName("iceServers")]
    public IEnumerable<IceServerModel> IceServers { get; set; }
}