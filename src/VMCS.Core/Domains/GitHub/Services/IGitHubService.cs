using VMCS.Core.Domains.GitHub.Models;

namespace VMCS.Core.Domains.GitHub.Services;

public interface IGitHubService
{
    Task Create(AccessToken accessToken);
    Task SignIn(string userId, FormUrlEncodedContent data);
    Task CreateRepository(CreateRepository createRepository);
    Task PushToRepository(PushToRepository pushToRepository);
}