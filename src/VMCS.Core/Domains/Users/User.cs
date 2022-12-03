using VMCS.Core.Domains.Channels;
using VMCS.Core.Domains.Chats;
using VMCS.Core.Domains.Meetings;

namespace VMCS.Core.Domains.Users
{
    public class User
    {
        public string Id { get; set; }
        public string Login { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        
        public List<Channel> Channels { get; set; }
        public List<Meeting> Meetings { get; set; }
        public List<Chat> Chats { get; set; }
    }
}
