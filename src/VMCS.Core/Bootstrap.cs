using Microsoft.Extensions.DependencyInjection;

namespace VMCS.Core;

public static class Bootstrap
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        return services;
    }
}