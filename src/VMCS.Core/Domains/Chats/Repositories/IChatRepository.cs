namespace VMCS.Core.Domains.Chats.Repositories;

public interface IChatRepository
{
    public Task Delete(string id, CancellationToken cancellationToken);
}