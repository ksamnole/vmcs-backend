using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VMCS.Core;
using VMCS.Core.Domains.ChannelInvitations.Repositories;
using VMCS.Core.Domains.Channels.Repositories;
using VMCS.Core.Domains.Chats.Repositories;
using VMCS.Core.Domains.CodeExecution.HttpClients;
using VMCS.Core.Domains.Directories.Repositories;
using VMCS.Core.Domains.GitHub.HttpClients;
using VMCS.Core.Domains.GitHub.Repositories;
using VMCS.Core.Domains.Meetings.Repositories;
using VMCS.Core.Domains.Messages.Repositories;
using VMCS.Core.Domains.Users.Repositories;
using VMCS.Data.ChannelInvitations.Repositories;
using VMCS.Data.Channels.Repositories;
using VMCS.Data.Chats.Repositories;
using VMCS.Data.Contexts;
using VMCS.Data.Directories;
using VMCS.Data.GitHub.Repositories;
using VMCS.Data.HttpClients;
using VMCS.Data.HttpClients.CodeExecution.JudgeZero;
using VMCS.Data.HttpClients.CodeSharing;
using VMCS.Data.HttpClients.GitHub;
using VMCS.Data.Meetings.Repositories;
using VMCS.Data.Messages.Repositories;
using VMCS.Data.Users.Repositories;
using VMCS.Data.WebSocketClients;

namespace VMCS.Data;

public static class Bootstrap
{
    public static IServiceCollection AddData(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IGitHubRepository, GitHubRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IChannelRepository, ChannelRepository>();
        services.AddScoped<IMeetingRepository, MeetingRepository>();
        services.AddScoped<IMessageRepository, MessageRepository>();
        services.AddScoped<IChatRepository, ChatRepository>();
        services.AddScoped<IChannelInvitationRepository, ChannelInvitationRepository>();
        services.AddScoped<IDirectoryRepository, DirectoryRepository>();
        services.AddScoped<ICodeExecutor, JudgeZeroCodeExecutor>();
        
        services.AddDbContext<AuthenticationContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("ConnectionString")));
        services.AddDbContext<ApplicationContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("ConnectionString")));

        services.AddHttpClient<CodeSharingApi>(options =>
        {
            options.BaseAddress = new Uri(configuration["CodeSharingApi:BaseUri"]);
        });
        
        services.AddHttpClient<IGitHubSignIn, GitHubSignIn>(options =>
        {
            options.BaseAddress = new Uri(configuration["GitHub:SignInUri"]);
        });
        services.AddHttpClient<IGitHubApi, GitHubApi>(options =>
        {
            options.BaseAddress = new Uri(configuration["GitHub:ApiUri"]);
            options.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; AcmeInc/1.0)");
        });
        services.AddHttpClient("JudgeZeroDefault",options =>
        {
            options.BaseAddress = new Uri(configuration["JudgeZero:ApiUri"]);
            options.DefaultRequestHeaders.Add("X-RapidAPI-Key", configuration["JudgeZero:RapidApiKey"]);
            options.DefaultRequestHeaders.Add("X-RapidAPI-Host", configuration["JudgeZero:RapidApiHost"]);
        });
        services.AddHttpClient("JudgeZeroExtra",options =>
        {
            options.BaseAddress = new Uri(configuration["JudgeZeroExtra:ApiUri"]);
            options.DefaultRequestHeaders.Add("X-RapidAPI-Key", configuration["JudgeZeroExtra:RapidApiKey"]);
            options.DefaultRequestHeaders.Add("X-RapidAPI-Host", configuration["JudgeZeroExtra:RapidApiHost"]);
        });

        services.AddSingleton<CodeSharingWs>();
        
        return services;
    }
}