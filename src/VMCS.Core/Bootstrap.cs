using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using VMCS.Core.Domains.Channels;
using VMCS.Core.Domains.Channels.Services;
using VMCS.Core.Domains.Channels.Validators;
using VMCS.Core.Domains.Users;
using VMCS.Core.Domains.Meetings.Services;
using VMCS.Core.Domains.Users.Services;
using VMCS.Core.Domains.Users.Validators;
using VMCS.Core.Domains.Messages.Services;

namespace VMCS.Core;

public static class Bootstrap
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddScoped<IValidator<User>, UserValidator>();
        services.AddScoped<IValidator<Channel>, ChannelValidator>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IChannelService, ChannelService>();
        services.AddScoped<IMeetingService, MeetingService>();
        services.AddScoped<IMessageService, MessageService>();

        return services;
    }
}