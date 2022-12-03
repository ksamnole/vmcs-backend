using System.Collections.Generic;
using VMCS.API.Controllers.Meetings.Dto;
using VMCS.Core.Domains.Chats;
using VMCS.Core.Domains.Users;

namespace VMCS.API.Controllers.Channel.Dto;

public class ChannelDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public Chat Chat { get; set; }
    public IEnumerable<User> Users { get; set; }
    public IEnumerable<ShortMeetingDto> Meetings { get; set; }
}