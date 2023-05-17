using System;
using AutoMapper;
using HealthyCountry.DataModels;
using HealthyCountry.Hubs.Models;
using HealthyCountry.Models;
using HealthyCountry.RTC.Entities;
using HealthyCountry.RTC.Entities.Chats;
using HealthyCountry.RTC.Models;
using HealthyCountry.RTC.Models.Chats;

namespace HealthyCountry.Utilities;

public class EntityToModelProfile : Profile
{
    public EntityToModelProfile()
    {
        CreateMap<UserRequestDataModel, User>().ReverseMap();

        CreateMap<User, User>()
            .ForMember(x => x.Password, o => o.Ignore())
            .ForMember(x => x.IsActive, o => o.Ignore());

        CreateMap<GroupModel, GroupEntity>()
            .ForMember(x => x.Id, o => o
                .MapFrom(s => s.Id))
            .ForMember(x => x.UserConnections, o => o
                .MapFrom(s => s.Users))
            .ReverseMap();

        CreateMap<UserModel, UserConnectionEntity>()
            .ForMember(x => x.RelativeUserId, o => o
                .MapFrom(s => s.Id))
            .ForMember(x => x.Role, o => o
                .MapFrom(s => s.Role))
            .ForMember(x => x.ConnectionId, o => o
                .MapFrom(s => s.ConnectionId))
            .ReverseMap();

        CreateMap<WebSocketEventModel, WebsocketEventEntity>()
            .ReverseMap();

        CreateMap<IceServerModel, IceServerResponseModel>();
        CreateMap<IceServerEntity, IceServerModel>()
            .ForMember(x => x.Urls, o =>
            {
                o.PreCondition(s => !string.IsNullOrEmpty(s.Urls));
                o.MapFrom(s => s.Urls.Split(',', StringSplitOptions.RemoveEmptyEntries));
            });
        CreateMap<IceServerModel, IceServerEntity>()
            .ForMember(x => x.Urls, o =>
            {
                o.PreCondition(s => s.Urls is {Count: > 0});
                o.MapFrom(s => string.Join(',', s.Urls));
            });

        CreateMap<AttachmentModel, AttachmentEntity>()
            .ReverseMap();

        CreateMap<ParticipantModel, ParticipantEntity>()
            .ForMember(x => x.Role, o => o
                .MapFrom(s => (byte) s.Role));
        CreateMap<ParticipantEntity, ParticipantModel>()
            .ForMember(x => x.Role, o => o
                .MapFrom(s => (Roles) s.Role));

        CreateMap<ExtendedParticipantModel, ExtendedParticipantEntity>()
            .ForMember(x => x.Role, o => o
                .MapFrom(s => (byte) s.Role));
        CreateMap<ExtendedParticipantEntity, ExtendedParticipantModel>()
            .ForMember(x => x.Role, o => o
                .MapFrom(s => (Roles) s.Role));

        CreateMap<ChatModel, ChatEntity>()
            .ForMember(x => x.ContextType, o => o
                .MapFrom(s => (byte) s.ContextType));
        CreateMap<ChatEntity, ChatModel>()
            .ForMember(x => x.ContextType, o => o
                .MapFrom(s => (ContextTypes) s.ContextType));

        CreateMap<MessageModel, MessageEntity>()
            .ForMember(x => x.ReadBy, o => o.MapFrom(s => s.ReadBy))
            .ReverseMap();

        CreateMap<ParticipantModel, ExtendedParticipantModel>();

        CreateMap<WebSocketUserModel, UserModel>()
            .ForMember(x => x.Id, o => o
                .MapFrom(s => s.RelativeId))
            .ForMember(x => x.Role, o => o
                .MapFrom(s => s.Role))
            .ForMember(x => x.ConnectionId, o => o
                .MapFrom(s => s.ConnectionId));

        // CreateMap<WebSocketUserModel, ParticipantModel>()
        //     .ForMember(x => x.Role,
        //         o => o.MapFrom(s =>
        //             EnvironmentConstants.HisClients.Contains(s.ClientId) ? Roles.Doctor : Roles.Patient))
        //     .ForMember(x => x.ResourceId, o => o
        //         .MapFrom(s => s.ResourceId))
        //     .ForMember(x => x.UserId, o => o
        //         .MapFrom(s => s.UserId));
    }
}