using Microsoft.Extensions.DependencyInjection;

namespace VMCS.Data;

public static class Bootstrap
{
    public static IServiceCollection AddData(this IServiceCollection services)
    {
        return services;
    }
}