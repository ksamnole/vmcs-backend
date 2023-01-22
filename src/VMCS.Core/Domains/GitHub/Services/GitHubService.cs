using System.Net.Http.Json;
using Newtonsoft.Json;
using VMCS.Core.Domains.Directories.Services;
using VMCS.Core.Domains.GitHub.HttpClients;
using VMCS.Core.Domains.GitHub.Models;
using VMCS.Core.Domains.GitHub.Repositories;

namespace VMCS.Core.Domains.GitHub.Services;

public class GitHubService : IGitHubService
{
    private readonly IGitHubRepository _gitHubRepository;
    private readonly IDirectoryService _directoryService;
    private readonly IGitHubSignIn _gitHubSignIn;
    private readonly IGitHubApi _gitHubApi;
    private readonly IUnitOfWork _unitOfWork;

    public GitHubService(IGitHubRepository gitHubRepository, IUnitOfWork unitOfWork, IGitHubSignIn gitHubSignIn, IGitHubApi gitHubApi, IDirectoryService directoryService)
    {
        _gitHubRepository = gitHubRepository;
        _unitOfWork = unitOfWork;
        _gitHubSignIn = gitHubSignIn;
        _gitHubApi = gitHubApi;
        _directoryService = directoryService;
    }

    public async Task Create(AccessToken accessToken)
    {
        await _gitHubRepository.Create(accessToken);
        await _unitOfWork.SaveChange();
    }

    public async Task SignIn(string userId, FormUrlEncodedContent data)
    {
        var token = await _gitHubSignIn.SignIn(data);

        await Create(new AccessToken()
        {
            Token = token,
            UserId = userId
        });
    }

    private async Task<AccessToken> GetToken(string userId)
    {
        return await _gitHubRepository.GetToken(userId);
    }

    public async Task CreateRepository(CreateRepository createRepository)
    {
        var accessToken = await GetToken(createRepository.UserId);
        var data = JsonContent.Create(createRepository);
        await _gitHubApi.CreateRepository("/user/repos", accessToken.Token, data);
    }

    public Task PushToRepository(PushToRepository pushToRepository)
    {
        throw new NotImplementedException();
    }
}