namespace HealthyCountry.Hubs.Models;

/// <summary>
/// RTC hub message
/// </summary>
public class HubMessage<TData> where TData : new()
{
    /// <summary>
    /// ctor
    /// </summary>
    public HubMessage()
    {
        Data = new TData();
    }

    /// <summary>
    /// Additional information
    /// </summary>
    public TData Data { get; set; }

    /// <summary>
    /// Reason
    /// </summary>
    public string Event { get; set; }

    /// <summary>
    /// Message type
    /// </summary>
    public MessageTypes MessageType { get; set; }
    
    /// <summary>
    /// Socket connection id. Possible to remove
    /// </summary>
    public string ConnectionId { get; set; }
}

public enum MessageTypes
{
    Message = 0,
    Notification = 1,
    CallResult = 2,
}