using System.Net.Http.Headers;
using System.Net.Http.Json;
using Newtonsoft.Json;
using VMCS.Core.Domains.GitHub.HttpClients;
using VMCS.Data.HttpClients.Models;
using VMCS.Data.HttpClients.Models.Responses;

namespace VMCS.Data.HttpClients.GitHub;

public class GitHubApi : IGitHubApi
{
    private readonly HttpClient _httpClient;

    public GitHubApi(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task CreateRepository(string url, string token, StringContent data)
    {
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
        
        var response = await _httpClient.PostAsync(url, data);

        if (!response.IsSuccessStatusCode)
            throw new Exception();
    }

    public async Task<string> GetUserLogin(string url, string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
        
        var response = await _httpClient.GetAsync(url);

        var deserializeObject = await GetDeserializeObject<UserResponse>(response);

        return deserializeObject.Login;
    }

    public async Task<string> GetMainBranchName(string url, string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
        
        var response = await _httpClient.GetAsync(url);
        
        var deserializeObject = await GetDeserializeObject<List<BranchResponse>>(response);

        return deserializeObject[0].Name;
    }

    public async Task<List<string>> GetAllUserRepositoriesNames(string url, string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
        
        var response = await _httpClient.GetAsync(url);
        
        var deserializeObject = await GetDeserializeObject<List<UserRepositoryResponse>>(response);

        return deserializeObject.Select(x => x.Name).ToList();
    }

    public async Task<string> GetShaBaseTree(string url, string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
        
        var response = await _httpClient.GetAsync(url);

        var deserializeObject = await GetDeserializeObject<ShaResponse>(response);

        return deserializeObject.Sha;
    } 
    
    public async Task<string> GetShaBlob(string url, JsonContent data, string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
        
        var response = await _httpClient.PostAsync(url, data);

        var deserializeObject = await GetDeserializeObject<ShaResponse>(response);

        return deserializeObject.Sha;
    }

    public async Task<string> GetShaTree(string url, StringContent data, string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
        
        var response = await _httpClient.PostAsync(url, data);
        
        var deserializeObject = await GetDeserializeObject<ShaResponse>(response);

        return deserializeObject.Sha;
    }
    
    public async Task<string> GetShaParent(string url, string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
        
        var response = await _httpClient.GetAsync(url);
        
        var deserializeObject = await GetDeserializeObject<ParentResponse>(response);

        return deserializeObject.Object.Sha;
    }
    
    public async Task<string> GetShaCommit(string url, JsonContent data, string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
        
        var response = await _httpClient.PostAsync(url, data);
        
        var deserializeObject = await GetDeserializeObject<ShaResponse>(response);

        return deserializeObject.Sha;
    }
    
    public async Task UpdateRef(string url, JsonContent data, string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
        
        var response = await _httpClient.PatchAsync(url, data);
        
        if (!response.IsSuccessStatusCode)
            throw new Exception();
    }

    private static async Task<T> GetDeserializeObject<T>(HttpResponseMessage requestMessage)
    {
        if (!requestMessage.IsSuccessStatusCode)
            throw new Exception();
        
        var responseContent = await requestMessage.Content.ReadAsStringAsync();
        var deserializeObject = JsonConvert.DeserializeObject<T>(responseContent);

        if (deserializeObject is null)
            throw new Exception();

        return deserializeObject;
    }
}