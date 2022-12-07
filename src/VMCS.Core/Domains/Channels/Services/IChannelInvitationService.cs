using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMCS.Core.Domains.Channels.Services
{
    public interface IChannelInvitationService
    {
        Task Accept(string invitationId, string userId, CancellationToken cancellationToken);
        Task Decline(string invitationId, string userId, CancellationToken cancellationToken);
        Task Create(ChannelInvitation invitation, CancellationToken cancellationToken);
        Task Delete(string invitaionId, CancellationToken cancellationToken);
    }
}
