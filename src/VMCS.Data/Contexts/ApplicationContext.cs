using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using VMCS.Core.Domains.ChannelInvitations;
using VMCS.Core.Domains.Channels;
using VMCS.Core.Domains.Chats;
using VMCS.Core.Domains.Meetings;
using VMCS.Core.Domains.Messages;
using VMCS.Core.Domains.Users;
using VMCS.Data.Contexts.Mapping;

namespace VMCS.Data.Contexts;

public class ApplicationContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Channel> Channels { get; set; }
    public DbSet<Meeting> Meetings { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Chat> Chats { get; set; }
    
    public DbSet<ChannelInvitation> ChannelInvitations { get; set; }
    
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().Setup().ToTable("Users");
        modelBuilder.Entity<Channel>().Setup().ToTable("Channels");
        modelBuilder.Entity<Meeting>().Setup().ToTable("Meetings");
        modelBuilder.Entity<Message>().Setup().ToTable("Messages");
        modelBuilder.Entity<Chat>().Setup().ToTable("Chats");

        modelBuilder.Entity<ChannelInvitation>().ToTable("ChannelInvitations");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.LogTo(Console.WriteLine, new[] { RelationalEventId.CommandExecuted });
    }

    public class Factory : IDesignTimeDbContextFactory<ApplicationContext>
    {
        public ApplicationContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory() + @"\..\VMCS.API")
                .AddJsonFile("appsettings.json")
                .Build();

            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseNpgsql(configuration.GetConnectionString("ConnectionString")).Options;

            return new ApplicationContext(options);
        }
    }
}