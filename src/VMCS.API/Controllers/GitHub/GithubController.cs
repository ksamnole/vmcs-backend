using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using VMCS.API.Controllers.GitHub.Dto;
using VMCS.Core.Domains.GitHub.Models;
using VMCS.Core.Domains.GitHub.Services;

namespace VMCS.API.Controllers.GitHub;

[ApiController]
[Route("github")]
public class GithubController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IGitHubService _gitHubService;

    public GithubController(IConfiguration configuration, IGitHubService gitHubService)
    {
        _configuration = configuration;
        _gitHubService = gitHubService;
    }

    [HttpGet]
    [Route("signin")]
    public async Task<string> SignIn(string code, string userId)
    {
        // https://github.com/login/oauth/authorize?client_id=c287f00c6d12fd1c2aad&redirect_uri=https%3A%2F%2Flocalhost%3A5001%2Fgithub%2Fsignin%3FuserId%3D9de5cd29-a809-464d-83a9-c4bda5800637&scope=repo&response_type=code

        var data = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("client_id", _configuration["GitHub:ClientId"]),
            new KeyValuePair<string, string>("client_secret", _configuration["GitHub:ClientSecret"]),
            new KeyValuePair<string, string>("code", code),
            new KeyValuePair<string, string>("redirect_uri", _configuration["RedirectUri"]),
            new KeyValuePair<string, string>("state", "")
        });

        await _gitHubService.SignIn(userId, data);

        return "Successful authentication on github. Go back to the application page.";
    }

    [HttpPost]
    [Route("repository/create")]
    public async Task CreateRepository(CreateRepositoryDto createRepositoryDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            throw new ValidationException("Please log in.");

        await _gitHubService.CreateRepository(new CreateRepository
        {
            UserId = userId,
            Name = createRepositoryDto.Name
        });
    }

    [HttpPost]
    [Route("repository/push")]
    public async Task PushToRepository(PushToRepositoryDto pushToRepositoryDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            throw new ValidationException("Please log in.");

        await _gitHubService.PushToRepository(new PushToRepository
        {
            DirectoryId = pushToRepositoryDto.DirectoryId,
            Message = pushToRepositoryDto.Message,
            RepositoryName = pushToRepositoryDto.RepositoryName,
            UserId = userId
        });
    }
}