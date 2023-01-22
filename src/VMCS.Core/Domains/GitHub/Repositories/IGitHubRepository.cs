namespace VMCS.Core.Domains.GitHub.Repositories;

public interface IGitHubRepository
{
    Task Create(AccessToken accessToken);
    Task<AccessToken> GetToken(string userId);
}