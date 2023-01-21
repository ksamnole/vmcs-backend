using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using VMCS.Core.Domains.ChannelInvitations.Services;
using VMCS.Core.Domains.Channels;
using VMCS.Core.Domains.Channels.Services;
using VMCS.Core.Domains.Channels.Validators;
using VMCS.Core.Domains.Chats.Services;
using VMCS.Core.Domains.CodeSharing.Models;
using VMCS.Core.Domains.CodeSharing.Validators;
using VMCS.Core.Domains.Directories.Services;
using VMCS.Core.Domains.Meetings;
using VMCS.Core.Domains.Meetings.Services;
using VMCS.Core.Domains.Meetings.Validators;
using VMCS.Core.Domains.Messages;
using VMCS.Core.Domains.Messages.Services;
using VMCS.Core.Domains.Messages.Validators;
using VMCS.Core.Domains.Users;
using VMCS.Core.Domains.Users.Services;
using VMCS.Core.Domains.Users.Validators;

namespace VMCS.Core;

public static class Bootstrap
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddScoped<IValidator<User>, UserValidator>();
        services.AddScoped<IValidator<Channel>, ChannelValidator>();
        services.AddScoped<IValidator<Meeting>, MeetingValidator>();
        services.AddScoped<IValidator<Message>, MessageValidator>();
        services.AddSingleton<IValidator<TextFile>, TextFileValidator>();

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IChannelService, ChannelService>();
        services.AddScoped<IMeetingService, MeetingService>();
        services.AddScoped<IMessageService, MessageService>();
        services.AddScoped<IChatService, ChatService>();
        services.AddScoped<IChannelInvitationService, ChannelInvitationService>();
        services.AddScoped<IDirectoryService, DirectoryService>();


        return services;
    }
}