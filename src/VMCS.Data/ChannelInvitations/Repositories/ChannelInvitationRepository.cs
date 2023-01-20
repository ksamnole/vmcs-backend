using System.Data.Entity.Core;
using Microsoft.EntityFrameworkCore;
using VMCS.Core.Domains.ChannelInvitations;
using VMCS.Core.Domains.ChannelInvitations.Repositories;
using VMCS.Data.Contexts;

namespace VMCS.Data.ChannelInvitations.Repositories;

public class ChannelInvitationRepository : IChannelInvitationRepository
{
    private readonly ApplicationContext _applicationContext;

    public ChannelInvitationRepository(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }

    public async Task<ChannelInvitation> Get(string id, CancellationToken cancellationToken)
    {
        var entity =
            await _applicationContext.ChannelInvitations.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (entity == null)
            throw new ObjectNotFoundException($"Invitation to channel with Id = {id} not found");

        return entity;
    }

    public async Task Create(ChannelInvitation channelInvitation, CancellationToken cancellationToken)
    {
        await _applicationContext.ChannelInvitations.AddAsync(channelInvitation, cancellationToken);
    }

    public async Task Delete(string id, CancellationToken cancellationToken)
    {
        var entity =
            await _applicationContext.ChannelInvitations.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (entity == null)
            throw new ObjectNotFoundException($"Invitation to channel with Id = {id} not found");

        _applicationContext.ChannelInvitations.Remove(entity);
    }
}