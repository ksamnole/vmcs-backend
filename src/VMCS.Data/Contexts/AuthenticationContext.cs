using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
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
                var options = new DbContextOptionsBuilder()
                    .UseNpgsql("Host=localhost;Port=5432;Database=vmcs;Username=vmcs;Password=qwerty321")
                    .Options;

                return new AuthenticationContext(options);
            }
        }
    }
}
