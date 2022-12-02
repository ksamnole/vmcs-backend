using VMCS.Core.Domains.Chats;
using VMCS.Core.Domains.Users;

namespace VMCS.Core.Domains.Messages
{
    public class Message
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string UserId { get; set; }
        public User User { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string ChatId { get; set; }
        public Chat Chat {get;set;}
    }
}
