using System.Net.Http.Headers;
using System.Net.Http.Json;
using VMCS.Core.Domains.GitHub.HttpClients;

namespace VMCS.Data.HttpClients.GitHub;

public class GitHubApi : IGitHubApi
{
    private readonly HttpClient _httpClient;

    public GitHubApi(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task CreateRepository(string url, string token, JsonContent data)
    {
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
        
        var response = await _httpClient.PostAsync(url, data);

        if (!response.IsSuccessStatusCode)
            throw new Exception();
    }

    public Task PushToRepository(string url, FormUrlEncodedContent data)
    {
        throw new NotImplementedException();
    }
}