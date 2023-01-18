using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMCS.Core.Domains.CodeSharing.Models
{
    public class Repository
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string MeetingId { get; set; }
        public string Name { get; set; }
        public Folder Directory { get; set; }
    }
}
