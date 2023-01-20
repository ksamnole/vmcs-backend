using VMCS.Core.Domains.Chats;
using VMCS.Core.Domains.Meetings;
using VMCS.Core.Domains.Users;

namespace VMCS.Core.Domains.Channels;

public class Channel : BaseEntity
{
    public Channel()
    {
        Chat = new Chat();
        Users = new List<User>();
        Meetings = new List<Meeting>();

        ChatId = Chat.Id;
    }

    public string Name { get; set; }
    public string ChatId { get; set; }
    public string CreatorId { get; set; }

    public User Creator { get; set; }
    public Chat Chat { get; set; }
    public virtual ICollection<User> Users { get; set; }
    public virtual ICollection<Meeting> Meetings { get; set; }
}