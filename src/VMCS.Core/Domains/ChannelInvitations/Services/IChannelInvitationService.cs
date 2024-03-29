﻿namespace VMCS.Core.Domains.ChannelInvitations.Services;

public interface IChannelInvitationService
{
    Task Accept(string invitationId, string userId, CancellationToken cancellationToken);
    Task Decline(string invitationId, string userId, CancellationToken cancellationToken);
    Task Create(ChannelInvitation invitation, CancellationToken cancellationToken);
    Task Delete(string invitaionId, string userId, CancellationToken cancellationToken);
}