using System.Text.Json.Serialization;

namespace VMCS.Data.HttpClients.Models.Responses;

public class SubmissionResponse
{
    [JsonPropertyName("submission_id")]
    public string SubmissionId { get; set; }
}