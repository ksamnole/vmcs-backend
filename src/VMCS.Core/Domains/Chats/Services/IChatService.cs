namespace VMCS.Core.Domains.Chats.Services;

public interface IChatService
{
    Task Delete(string id, CancellationToken cancellationToken);
}