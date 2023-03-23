using System.Collections.Generic;
using VMCS.API.Controllers.Chats.Dto;
using VMCS.API.Controllers.Meetings.Dto;
using VMCS.API.Controllers.Users.Dto;

namespace VMCS.API.Controllers.Channels.Dto;

public class ChannelDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public ShortUserDto Creator { get; set; }
    public ShortChatDto Chat { get; set; }
    public IEnumerable<ShortUserDto> Users { get; set; }
    public IEnumerable<ShortMeetingDto> Meetings { get; set; }
    public string AvatarUri { get; set; }
}