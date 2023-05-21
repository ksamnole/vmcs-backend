using Newtonsoft.Json;

namespace VMCS.Data.HttpClients.Models.Responses;

internal class SubmissionResponse
{
    [JsonProperty("stdout")] public string Stdout { get; set; }

    [JsonProperty("status")] public string Status { get; set; }
}