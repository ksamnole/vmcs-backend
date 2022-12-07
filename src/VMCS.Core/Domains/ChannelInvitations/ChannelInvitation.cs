using VMCS.Core.Domains.Channels;
using VMCS.Core.Domains.Users;

namespace VMCS.Core.Domains.ChannelInvitations;

public class ChannelInvitation : BaseEntity
{
    public string SenderId { get; set; }
    public User Sender { get; set; }

    public string RecipientId { get; set; }
    public User Recipient { get; set; }

    public string ChannelId { get; set; }
    public Channel Channel { get; set; }
}