using VMCS.Core.Domains.Chats;
using VMCS.Core.Domains.Users;

namespace VMCS.Core.Domains.Messages
{
    public class Message
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public User User { get; set; }
        public Chat Chat { get; set;}
    }
}
