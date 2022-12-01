using Microsoft.Extensions.DependencyInjection;
using VMCS.Core.Domains.Channels.Services;
using VMCS.Core.Domains.Users.Services;

namespace VMCS.Core;

public static class Bootstrap
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IChannelService, ChannelService>();

        return services;
    }
}