﻿using Microsoft.EntityFrameworkCore;
using VMCS.Data.Channels;
using VMCS.Data.Meetings;
using VMCS.Data.Messages;
using VMCS.Data.Users;

namespace VMCS.Data.Contexts;

public class ApplicationContext : DbContext
{
    // TODO: Create DbContext
    public DbSet<UserDbModel> Users { get; set; }
    public DbSet<ChannelDbModel> Channels { get; set; }
    public DbSet<MeetingDbModel> Meetings { get; set; }
    public DbSet<MessageDbModel> Messages { get; set; }
    
    public ApplicationContext(DbContextOptions options) : base(options)
    {
    }
}