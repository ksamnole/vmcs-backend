using VMCS.Core.Domains.Channels;
using VMCS.Core.Domains.Chats;
using VMCS.Core.Domains.Meetings;

namespace VMCS.Core.Domains.Users
{
    public class User : BaseEntity
    {
        public string Login { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        
        public virtual ICollection<Channel> Channels { get; set; }
        public virtual ICollection<Meeting> Meetings { get; set; }

        public User()
        {
            Channels = new List<Channel>();
            Meetings = new List<Meeting>();
        }
    }
}
