using VMCS.Core.Domains.Users;

namespace VMCS.Core.Domains.Chats
{
    public class Chat
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public List<User> Users { get; set; }
    }
}
