using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VMCS.Core.Domains.Auth;

namespace VMCS.Data.Contexts
{
    public class AuthenticationContext : IdentityDbContext<AuthUser>
    {
        public AuthenticationContext(DbContextOptions<AuthenticationContext> options) : base(options)
        {
        }
    }
}
