using System.Collections.Generic;
using System.Threading.Tasks;
using HealthyCountry.RTC.Models;

namespace HealthyCountry.RTC.Interfaces.Repositories;

public interface IIceServerRepository
{         
    Task<List<IceServerModel>> GetServers(string userId);

    Task CreateOrUpdateIceServer(IceServerModel iceServer);
}