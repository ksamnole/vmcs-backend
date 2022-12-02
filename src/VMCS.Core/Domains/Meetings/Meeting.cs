using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMCS.Core.Domains.Channels;
using VMCS.Core.Domains.Users;

namespace VMCS.Core.Domains.Meetings
{
    public class Meeting
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsInChannel { get; set; }

        public string UserId { get; set; }
        public User user { get; set; }

        public string ChannelId { get; set; }
        public Channel Channel { get; set; }
    }

    
}
