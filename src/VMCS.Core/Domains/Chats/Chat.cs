﻿using VMCS.Core.Domains.Channels;
using VMCS.Core.Domains.Meetings;
using VMCS.Core.Domains.Messages;

namespace VMCS.Core.Domains.Chats;

public class Chat : BaseEntity
{
    public Chat()
    {
        Messages = new List<Message>();
    }

    public string? ChannelId { get; set; }
    public string? MeetingId { get; set; }
    public Channel? Channel { get; set; }
    public Meeting? Meeting { get; set; }

    public virtual ICollection<Message> Messages { get; set; }
}