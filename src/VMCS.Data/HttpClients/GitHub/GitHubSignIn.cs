using System.Security.Authentication;
using System.Text.RegularExpressions;
using VMCS.Core.Domains.GitHub.HttpClients;

namespace VMCS.Data.HttpClients.GitHub;

public class GitHubSignIn : IGitHubSignIn
{
    private static readonly Regex Pattern = new (@"access_token=([A-Za-z0-9_]*)&", RegexOptions.Compiled);
    private readonly HttpClient _httpClient;

    public GitHubSignIn(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> SignIn(FormUrlEncodedContent data)
    {
        var response = await _httpClient.PostAsync("", data);

        if (!response.IsSuccessStatusCode)
            throw new AuthenticationException();
            
        var responseString = await response.Content.ReadAsStringAsync();
        return Pattern.Match(responseString).Groups[1].Value;
    }
}