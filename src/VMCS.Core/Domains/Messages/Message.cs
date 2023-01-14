using VMCS.Core.Domains.Chats;
using VMCS.Core.Domains.Users;

namespace VMCS.Core.Domains.Messages
{
    public class Message : BaseEntity
    {
        public string Text { get; set; }
        public string Username { get; set; }
        public string? UserId { get; set; }
        public string ChatId { get; set; }
        public User? User { get; set; }
        public Chat Chat { get; set;}
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
    }
}
