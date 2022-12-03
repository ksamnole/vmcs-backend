using System.Collections.Generic;
using VMCS.Core.Domains.Chats;
using VMCS.Core.Domains.Users;

namespace VMCS.API.Controllers.Meetings.Dto
{
    public class MeetingDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Chat Chat { get; set; }
        public IEnumerable<User> Users { get; set; }
    }
}
