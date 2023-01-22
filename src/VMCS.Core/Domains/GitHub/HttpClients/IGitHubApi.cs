using System.Net.Http.Json;

namespace VMCS.Core.Domains.GitHub.HttpClients;

public interface IGitHubApi
{
    Task CreateRepository(string url, string token, JsonContent data);
    Task PushToRepository(string url, FormUrlEncodedContent data);
}