using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
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
    
    public ApplicationContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().Setup().ToTable("Users");
        modelBuilder.Entity<Channel>().Setup().ToTable("Channels");
        modelBuilder.Entity<Meeting>().Setup().ToTable("Meetings");
        modelBuilder.Entity<Message>().Setup().ToTable("Messages");
        modelBuilder.Entity<Chat>().Setup().ToTable("Chats");
    }
    
    public class Factory : IDesignTimeDbContextFactory<ApplicationContext>
    {
        public ApplicationContext CreateDbContext(string[] args)
        {
            var options = new DbContextOptionsBuilder()
                .UseNpgsql("Host=localhost;Port=5432;Database=vmcs;Username=vmcs;Password=qwerty321")
                .Options;

            return new ApplicationContext(options);
        }
    }
}