using Microsoft.AspNetCore.Identity;

namespace VMCS.Core.Domains.Auth;

public class AuthUser : IdentityUser
{
    public string GivenName { get; set; }
}