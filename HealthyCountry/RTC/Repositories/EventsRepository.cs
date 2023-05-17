using System.Threading.Tasks;
using AutoMapper;
using HealthyCountry.RTC.Entities;
using HealthyCountry.RTC.Interfaces.Repositories;
using HealthyCountry.RTC.Models;

namespace HealthyCountry.RTC.Repositories
{
    public class EventsRepository : IEventsRepository
    {
        private readonly WebsocketContext _context;
        private readonly IMapper _mapper;

        public EventsRepository(WebsocketContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task CreateEvent(WebSocketEventModel eventItem)
        {
            var mongoEvent = _mapper.Map<WebsocketEventEntity>(eventItem);

            await _context.WebsocketEvents.InsertOneAsync(mongoEvent);
        }
    }
}
