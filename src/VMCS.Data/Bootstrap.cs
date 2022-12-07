using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VMCS.Core;
using VMCS.Core.Domains.ChannelInvitations.Repositories;
using VMCS.Core.Domains.Channels.Repositories;
using VMCS.Core.Domains.Chats.Repositories;
using VMCS.Core.Domains.Meetings.Repositories;
using VMCS.Core.Domains.Messages.Repositories;
using VMCS.Core.Domains.Users.Repositories;
using VMCS.Data.ChannelInvitations.Repositories;
using VMCS.Data.Channels.Repositories;
using VMCS.Data.Chats.Repositories;
using VMCS.Data.Contexts;
using VMCS.Data.Meetings.Repositories;
using VMCS.Data.Messages.Repositories;
using VMCS.Data.Users.Repositories;

namespace VMCS.Data;

public static class Bootstrap
{
    public static IServiceCollection AddData(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IChannelRepository, ChannelRepository>();
        services.AddScoped<IMeetingRepository, MeetingRepository>();
        services.AddScoped<IMessageRepository, MessageRepository>();
        services.AddScoped<IChatRepository, ChatRepository>();
        services.AddScoped<IChannelInvitationRepository, ChannelInvitationRepository>();
        
        services.AddDbContext<AuthenticationContext>(options => 
            options.UseNpgsql(configuration.GetConnectionString("ConnectionString")));
        services.AddDbContext<ApplicationContext>(options => 
            options.UseNpgsql(configuration.GetConnectionString("ConnectionString")));
        
        return services;
    }
}