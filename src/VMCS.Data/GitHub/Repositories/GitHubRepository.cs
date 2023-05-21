using System.Data.Entity.Core;
using Microsoft.EntityFrameworkCore;
using VMCS.Core.Domains.GitHub;
using VMCS.Core.Domains.GitHub.Repositories;
using VMCS.Data.Contexts;

namespace VMCS.Data.GitHub.Repositories;

public class GitHubRepository : IGitHubRepository
{
    private readonly ApplicationContext _applicationContext;

    public GitHubRepository(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }

    public async Task Create(AccessToken accessToken)
    {
        var entity = await _applicationContext.AccessTokens.FirstOrDefaultAsync(x => x.UserId == accessToken.UserId);

        if (entity is null)
            await _applicationContext.AccessTokens.AddAsync(accessToken);
        else
            entity.Token = accessToken.Token;
    }

    public async Task<AccessToken> GetToken(string userId)
    {
        var accessToken = await _applicationContext.AccessTokens.FirstOrDefaultAsync(x => x.UserId == userId);

        if (accessToken is null)
            throw new ObjectNotFoundException($"AccessToken with userId = {userId} not found.");

        return accessToken;
    }
}