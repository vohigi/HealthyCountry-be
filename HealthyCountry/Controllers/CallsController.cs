using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HealthyCountry.Hubs;
using HealthyCountry.Hubs.Models;
using HealthyCountry.RTC.Interfaces.Services;
using HealthyCountry.RTC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace HealthyCountry.Controllers;

[Produces("application/json")]
[Route("api/websocket/v1/sockethub")]
public class CallsController : ControllerBase
{
    private readonly IConferenceService _conferenceService;
    private readonly IHubContext<RtcHub, IRtcHub> _hubContext;

    public CallsController(
        IConferenceService conferenceService, 
        IHubContext<RtcHub, IRtcHub> hubContext)
    {
        _conferenceService = conferenceService;
        _hubContext = hubContext;
    }

    /// <summary>
    /// Get rtc config
    /// </summary> 
    /// <returns></returns>
    [HttpGet("rtc/config")]
    public async Task<WebRtcConfigModel> GetWebRtcConfig()
    {
        var serviceResponse = await _conferenceService.GetWebRtcConfigAsync(GetUserId(User));
        return serviceResponse.Result;
    }

    /// <summary>
    /// Get rooms information
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("rooms/info")]
    public async Task<RoomsInfoResponseModel> GetRoomsInfo([FromBody] RoomsInfoRequestModel request)
    {
        return await _conferenceService.GetGroupsInfoAsync(request);
    }

    /// <summary>
    /// Get room information
    /// </summary>
    /// <param name="roomId"></param>
    /// <returns></returns>
    [HttpGet("rooms/{roomId}")]
    public async Task<GroupModel> GetRoomsInfo(string roomId)
    {
        return await _conferenceService.GetOneGroupAsync(roomId);
    }
    private static string GetUserId(ClaimsPrincipal user) =>
        user?.Claims.Where(x => x.Type == ClaimTypes.Name).Select(x => x.Value).FirstOrDefault();
}