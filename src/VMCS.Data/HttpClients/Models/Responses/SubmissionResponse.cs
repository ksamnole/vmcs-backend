using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMCS.Data.HttpClients.Models.Responses
{
    internal class SubmissionResponse
    {
        [JsonProperty("stdout")]
        public string Stdout { get;set; }
        [JsonProperty("status")]
        public string Status { get; set; }
    }
}
