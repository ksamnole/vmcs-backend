using VMCS.Core.Domains.Channels;

namespace VMCS.Core.Domains.Users.Services
{
    public interface IUserService
    {
        Task<User> GetById(string id, CancellationToken cancellationToken);
        Task<IEnumerable<User>> GetAll(CancellationToken cancellationToken);
        Task<IEnumerable<Channel>> GetAllUserChannels(string userId, CancellationToken cancellationToken);
        Task<IEnumerable<ChannelInvitation>> GetAllUserChannelInvitations(string userId,
            CancellationToken cancellationToken);
        Task Create(User user, CancellationToken cancellationToken);
        Task Update(User user, CancellationToken cancellationToken);
        Task Delete(string id, CancellationToken cancellationToken);
    }
}
