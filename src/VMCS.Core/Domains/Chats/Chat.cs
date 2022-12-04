using VMCS.Core.Domains.Messages;

namespace VMCS.Core.Domains.Chats
{
    public class Chat
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        public IEnumerable<Message> Messages { get; set; }
    }
}
