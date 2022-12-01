﻿using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VMCS.Core;
using VMCS.Core.Domains.Users.Repositories;
using VMCS.Data.Contexts;
using VMCS.Data.Users.Repositories;

namespace VMCS.Data;

public static class Bootstrap
{
    public static IServiceCollection AddData(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserRepository, UserRepository>();
        
        services.AddDbContext<AuthenticationContext>(options => 
            options.UseNpgsql(configuration.GetConnectionString("ConnectionString")));
        services.AddDbContext<ApplicationContext>(options => 
            options.UseNpgsql(configuration.GetConnectionString("ConnectionString")));
        
        return services;
    }
}