using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using VMCS.Core.Domains.Auth;

namespace VMCS.Data.Contexts
{
    public class AuthenticationContext : IdentityDbContext<AuthUser>
    {
        public AuthenticationContext(DbContextOptions options) : base(options)
        {
        }
        
        public class Factory : IDesignTimeDbContextFactory<AuthenticationContext>
        {
            public AuthenticationContext CreateDbContext(string[] args)
            {
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory() + @"\..\VMCS.API")
                    .AddJsonFile("appsettings.json")
                    .Build();

                var options = new DbContextOptionsBuilder<ApplicationContext>()
                    .UseNpgsql(configuration.GetConnectionString("ConnectionString")).Options;

                return new AuthenticationContext(options);
            }
        }
    }
}
