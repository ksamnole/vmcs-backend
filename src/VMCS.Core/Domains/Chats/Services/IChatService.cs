namespace VMCS.Core.Domains.Chats.Services;

public interface IChatService
{
    Task Create(Chat chat, CancellationToken cancellationToken);
}