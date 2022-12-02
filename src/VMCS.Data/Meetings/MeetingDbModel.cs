using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;
using VMCS.Core.Domains.Channels;

namespace VMCS.Data.Meetings
{
    public class MeetingDbModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsInChannel { get; set; }
        public Channel Channel { get; set; }
    }
}
