using VMCS.Core.Domains.Users;

namespace VMCS.Core.Domains.GitHub;

public class AccessToken
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Token { get; set; }

    public string UserId { get; set; }
    public User User { get; set; }
}