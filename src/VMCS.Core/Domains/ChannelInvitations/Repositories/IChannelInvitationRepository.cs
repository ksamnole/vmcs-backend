using VMCS.Core.Domains.Channels;

namespace VMCS.Core.Domains.ChannelInvitations.Repositories;

public interface IChannelInvitationRepository
{
    Task Create(ChannelInvitation channelInvitation, CancellationToken cancellationToken);
    Task Delete(string id, CancellationToken cancellationToken);
}