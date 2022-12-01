using Microsoft.Extensions.DependencyInjection;
using VMCS.Core.Domains.Users.Repositories;
using VMCS.Data.Users.Repositories;

namespace VMCS.Data;

public static class Bootstrap
{
    public static IServiceCollection AddData(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        
        return services;
    }
}