using System.Collections.Generic;
using System.Threading.Tasks;
using HealthyCountry.Hubs.Models;
using HealthyCountry.RTC.Models.Chats;

namespace HealthyCountry.Hubs;

public interface IRtcHub
{
    Task ReceiveConferenceEvent(HubMessage<Dictionary<string, object>> socketMessage);
    
    Task ReceiveChatMessage(HubMessage<MessageModel> socketMessage);
    Task ReceiveChatEvent(HubMessage<Dictionary<string, object>> socketMessage);
    
    Task SendMessage(HubMessage<CallData> socketMessage);
    Task ReceiveNotification(HubMessage<Dictionary<string, object>> socketMessage);
}