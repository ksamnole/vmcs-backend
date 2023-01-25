using System.Data.Entity.Core;
using Microsoft.EntityFrameworkCore;
using VMCS.Core.Domains.Channels;
using VMCS.Core.Domains.Channels.Repositories;
using VMCS.Core.Domains.Users;
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

        entity.Chat.Messages = entity.Chat.Messages.OrderByDescending(x => x.CreatedAt).ToList();

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

    public async Task AddUser(User user, Channel channel, CancellationToken cancellationToken)
    {
        var entity = await _applicationContext.Channels.FirstOrDefaultAsync(x => x.Id == channel.Id);

        if (entity == null)
            throw new ObjectNotFoundException($"Channel with id = {channel.Id} not found");

        entity.Users.Add(user);
    }

    public async Task Update(Channel channel, CancellationToken cancellationToken)
    {
        var entity = await _applicationContext.Channels.FirstOrDefaultAsync(x => x.Id == channel.Id);

        if (entity == null)
            throw new ObjectNotFoundException($"Channel with id = {channel.Id} not found");

        entity.AvatarUri = channel.AvatarUri;
        entity.Name = channel.Name;
    }
}