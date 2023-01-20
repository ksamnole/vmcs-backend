using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using VMCS.Core.Domains.Auth;

namespace VMCS.Data.Contexts;

public class AuthenticationContext : IdentityDbContext<AuthUser>
{
    public AuthenticationContext(DbContextOptions<AuthenticationContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.LogTo(Console.WriteLine, new[] { RelationalEventId.CommandExecuted });
    }

    public class Factory : IDesignTimeDbContextFactory<AuthenticationContext>
    {
        public AuthenticationContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory() + @"\..\VMCS.API")
                .AddJsonFile("appsettings.json")
                .Build();

            var options = new DbContextOptionsBuilder<AuthenticationContext>()
                .UseNpgsql(configuration.GetConnectionString("ConnectionString")).Options;

            return new AuthenticationContext(options);
        }
    }
}