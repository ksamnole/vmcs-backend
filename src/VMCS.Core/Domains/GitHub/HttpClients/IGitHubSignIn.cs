namespace VMCS.Core.Domains.GitHub.HttpClients;

public interface IGitHubSignIn
{
    Task<string> SignIn(FormUrlEncodedContent data);
}