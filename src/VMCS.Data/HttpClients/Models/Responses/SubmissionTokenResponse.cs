using Newtonsoft.Json;

namespace VMCS.Data.HttpClients.Models.Responses;

public class SubmissionTokenResponse
{
    [JsonProperty("token")] public string Token { get; set; }
}