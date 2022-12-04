using System.Data.Entity.Core;
using Microsoft.EntityFrameworkCore;
using VMCS.Core.Domains.Channels;
using VMCS.Core.Domains.Channels.Repositories;
using VMCS.Data.Contexts;

namespace VMCS.Data.Channels.Repositories;

public class ChannelRepository : IChannelRepository
{
    private readonly ApplicationContext _applicationContext;

    public ChannelRepository(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }

    public async Task<Channel> GetById(string id, CancellationToken cancellationToken)
    {
        var entity = await _applicationContext.Channels
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        
        if (entity == null)
            throw new ObjectNotFoundException($"Channel with id = {id} not found");
        
        await _applicationContext.Chats.LoadAsync(cancellationToken);
        await _applicationContext.Entry(entity).Collection(c => c.Users).LoadAsync(cancellationToken);
        await _applicationContext.Meetings.LoadAsync(cancellationToken);
        await _applicationContext.Messages.LoadAsync(cancellationToken);

        return entity;
    }

    public async Task Create(Channel channel, CancellationToken cancellationToken)
    {
        await _applicationContext.Channels.AddAsync(channel, cancellationToken);
    }

    public async Task Delete(string id, CancellationToken cancellationToken)
    {
        var entity = await _applicationContext.Channels.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (entity == null)
            throw new ObjectNotFoundException($"Channel with id = {id} not found");

        _applicationContext.Channels.Remove(entity);
    }
}