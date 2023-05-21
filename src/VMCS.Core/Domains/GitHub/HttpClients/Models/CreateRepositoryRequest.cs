using Newtonsoft.Json;

namespace VMCS.Core.Domains.GitHub.HttpClients.Models;

public class CreateRepositoryRequest
{
    [JsonProperty("name")] public string Name { get; set; }

    [JsonProperty("private")] public string Private { get; set; }

    [JsonProperty("auto_init")] public bool AutoInit { get; set; }
}