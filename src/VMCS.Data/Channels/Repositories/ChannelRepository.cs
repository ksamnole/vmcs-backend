using System.Data.Entity;
using System.Data.Entity.Core;
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
        var entity = await _applicationContext.Channels.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (entity == null)
            throw new ObjectNotFoundException($"Канал с id = {id} не найден");

        return new Channel()
        {
            Id = entity.Id,
            Name = entity.Name
        };
    }

    public async Task Create(Channel channel, CancellationToken cancellationToken)
    {
        var entity = new ChannelDbModel()
        {
            Id = channel.Id,
            Name = channel.Name
        };

        await _applicationContext.Channels.AddAsync(entity, cancellationToken);
    }

    public async Task Delete(string id, CancellationToken cancellationToken)
    {
        var entity = await _applicationContext.Channels.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (entity == null)
            throw new ObjectNotFoundException($"Канал с id = {id} не найден");

        _applicationContext.Channels.Remove(entity);
    }
}