using VMCS.Core.Domains.Channels;
using VMCS.Core.Domains.Chats;
using VMCS.Core.Domains.Users;

namespace VMCS.Core.Domains.Meetings
{
    public class Meeting
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ClosedAt { get; set; }
        public bool IsInChannel { get; set; }
        public string CreatorId { get; set; }
        public string? ChannelId { get; set; }
        public string ChatId { get; set; }

        public User Creator { get; set; }
        public Channel? Channel { get; set; }
        public Chat Chat { get; set; }
        public IEnumerable<User> Users { get; set; }
    }
}
