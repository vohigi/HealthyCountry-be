using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HealthyCountry.RTC.Entities.Chats;
using HealthyCountry.RTC.Interfaces.Repositories;
using HealthyCountry.RTC.Models.Chats;
using HealthyCountry.RTC.Models.Chats.Requests;
using MongoDB.Driver;

namespace HealthyCountry.RTC.Repositories;

public class ChatsRepository : IChatsRepository
{
    private readonly WebsocketContext _context;
    private readonly IMapper _mapper;

    public ChatsRepository(
        WebsocketContext context, 
        IMapper mapper)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<ChatModel> GetOneChatAsync(Guid id)
    {
        return _mapper.Map<ChatModel>(await _context.Chats.Find(x => x.Id == id).FirstOrDefaultAsync());
    }

    public async Task<ChatModel> GetOneEventChatAsync(string eventId)
    {
        return _mapper.Map<ChatModel>(await _context.Chats
            .Find(x => x.ContextId == eventId && x.ContextType == (byte) ContextTypes.Event).FirstOrDefaultAsync());
    }
    
    public async Task<IEnumerable<ChatModel>> GetEventChatsAsync(IEnumerable<string> eventIds)
    {
        return (await _context.Chats
                .Find(x => eventIds.Contains(x.ContextId) && x.ContextType == (byte) ContextTypes.Event)
                .ToListAsync())
            .Select(_mapper.Map<ChatModel>);
    }

    public async Task CreateChatAsync(ChatModel model)
    {
        await _context.Chats.InsertOneAsync(_mapper.Map<ChatEntity>(model));
    }

    public async Task IncrementCountersAsync(Guid chatId, ParticipantModel participant, bool incrementSentMessages,
        bool incrementReadMessages, bool incrementSentAttachments)
    {
        var filter = Builders<ChatEntity>.Filter.Where(x =>
            x.Id == chatId && x.Participants.Any(s =>
                s.ResourceId == participant.RelativeUserId || s.UserId == participant.RelativeUserId));

        var updateDefinitions = new List<UpdateDefinition<ChatEntity>>();
        var updateBuilder = Builders<ChatEntity>.Update;

        if (incrementSentMessages || incrementSentAttachments)
        {
            updateDefinitions.Add(updateBuilder.Inc(x => x.MessagesCount, 1));
        }
        if (incrementSentMessages)
        {
            updateDefinitions.Add(updateBuilder.Inc(x => x.Participants.ElementAt(-1).SentMessagesCount, 1));
        }
        
        if (incrementReadMessages)
        {
            updateDefinitions.Add(updateBuilder.Inc(x => x.Participants.ElementAt(-1).ReadMessagesCount, 1));
        }
        
        if (incrementSentAttachments)
        {
            updateDefinitions.Add(updateBuilder.Inc(x => x.Participants.ElementAt(-1).SentAttachmentsCount, 1));
        }

        await _context.Chats.FindOneAndUpdateAsync(filter, updateBuilder.Combine(updateDefinitions));
    }

    public async Task<(bool hasNext, List<MessageModel>)> GetMessagesAsync(GetMessagesRequest request)
    {
        var sortDefinition =
            Builders<MessageEntity>.Sort.Descending(s => s.CreatedAt);
        
        var messageEntities = await _context.Messages
            .Find(x => x.ChatId == request.ChatId)
            .Sort(sortDefinition)
            .Skip(request.Skip)
            .Limit(request.Limit + 1)
            .ToListAsync();

        var hasNext = messageEntities.Count > request.Limit;

        return (hasNext,
            messageEntities
                .Take(request.Limit)
                .Select(x => ToPersonalizedMessageModel(x, _mapper, request.Requester))
                .ToList());
    }

    public async Task<MessageModel> GetOneMessageAsync(Guid id)
    {
        return _mapper.Map<MessageModel>(
            await _context.Messages.Find(x => x.Id == id).FirstOrDefaultAsync());
    }

    public async Task SaveMessageAsync(MessageModel model)
    {
        await _context.Messages.InsertOneAsync(_mapper.Map<MessageEntity>(model));
    }

    public async Task UpdateMessageAsync(MessageModel model)
    {
        await _context.Messages.FindOneAndReplaceAsync(x => x.Id == model.Id, 
            _mapper.Map<MessageEntity>(model));
    }

    private static MessageModel ToPersonalizedMessageModel(MessageEntity entity, IMapper mapper, ParticipantModel requester)
    {
        var model = mapper.Map<MessageModel>(entity);

        model.IsReadByRequester = model.ReadBy.Contains(requester.RelativeUserId);

        return model;
    }
}