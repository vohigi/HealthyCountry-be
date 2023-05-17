using System.Threading.Tasks;
using HealthyCountry.RTC.Models;

namespace HealthyCountry.RTC.Interfaces.Repositories;

public interface IEventsRepository
{         
    Task CreateEvent(WebSocketEventModel events); 
}