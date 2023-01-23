using System.Net.Http.Json;


namespace VMCS.Core.Domains.GitHub.HttpClients;

public interface IGitHubApi
{
    Task CreateRepository(string url, string token, StringContent data);
    Task<string> GetUserLogin(string url, string token);
    Task<string> GetMainBranchName(string url, string token);
    Task<List<string>> GetAllUserRepositoriesNames(string url, string token);
    Task<string> GetShaBaseTree(string url, string token);
    Task<string> GetShaBlob(string url, JsonContent data, string token);
    Task<string> GetShaTree(string url, StringContent data, string token);
    Task<string> GetShaParent(string url, string token);
    Task<string> GetShaCommit(string url, JsonContent data, string token);
    Task UpdateRef(string url, JsonContent data, string token);
}