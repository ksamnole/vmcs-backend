using VMCS.Core.Domains.Chats;
using VMCS.Core.Domains.Meetings;
using VMCS.Core.Domains.Users;

namespace VMCS.Core.Domains.Channels;

public class Channel
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; }
    public string ChatId { get; set; }
    public string CreatorId { get; set; }
    
    public User Creator { get; set; }
    public Chat Chat { get; set; }
    public List<User> Users { get; set; }
    public List<Meeting> Meetings { get; set; }
}