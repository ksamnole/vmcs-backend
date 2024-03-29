﻿using VMCS.Core.Domains.Channels;
using VMCS.Core.Domains.Meetings;

namespace VMCS.Core.Domains.Users;

public class User : BaseEntity
{
    public User()
    {
        Channels = new List<Channel>();
        Meetings = new List<Meeting>();
    }

    public string Login { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string? AvatarUri { get; set; }

    public virtual ICollection<Channel> Channels { get; set; }
    public virtual ICollection<Meeting> Meetings { get; set; }
}