using VMCS.Core.Domains.Users;

namespace VMCS.Core.Domains.Channels.Repositories;

public interface IChannelRepository
{
    Task<Channel> GetById(string id, CancellationToken cancellationToken);
    Task Create(Channel channel, CancellationToken cancellationToken);
    Task Delete(string id, CancellationToken cancellationToken);
    Task AddUser(User user, Channel channel, CancellationToken cancellationToken);
}