using VMCS.Core.Domains.Chats;
using VMCS.Core.Domains.Meetings;
using VMCS.Core.Domains.Users;

namespace VMCS.Core.Domains.Channels;

public class Channel
{
    public string Id { get; set; }
    public string Name { get; set; }
    public Chat Chat { get; set; }
    public User Creator { get; set; }
    
    public List<User> Users { get; set; }
    public List<Meeting> Meetings { get; set; }
}