using System.Text.Json.Serialization;

namespace VMCS.Core.Domains.CodeExecution.Enums;

[Newtonsoft.Json.JsonConverter(typeof(JsonStringEnumConverter))]
public enum Language
{
    Csharp,
    Python
}