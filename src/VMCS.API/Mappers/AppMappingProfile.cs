using AutoMapper;
using VMCS.API.Controllers.Channels.Dto;
using VMCS.API.Controllers.Chats.Dto;
using VMCS.API.Controllers.Meetings.Dto;
using VMCS.API.Controllers.Messages.Dto;
using VMCS.API.Controllers.Users.Dto;
using VMCS.Core.Domains.Channels;
using VMCS.Core.Domains.Chats;
using VMCS.Core.Domains.Meetings;
using VMCS.Core.Domains.Messages;
using VMCS.Core.Domains.Users;

namespace VMCS.API.Mappers;

public class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {
        CreateMap<User, ShortUserDto>();
        CreateMap<Message, ShortMessageDto>();
        CreateMap<Chat, ShortChatDto>();
        CreateMap<Meeting, ShortMeetingDto>();
        CreateMap<Channel, ShortChannelDto>();

        CreateMap<Message, MessageDto>();
    }
}