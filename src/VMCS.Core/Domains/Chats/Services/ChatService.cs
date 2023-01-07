using VMCS.Core.Domains.Chats.Repositories;

namespace VMCS.Core.Domains.Chats.Services;

public class ChatService : IChatService
{
    private readonly IChatRepository _chatRepository;

    public ChatService(IChatRepository chatRepository)
    {
        _chatRepository = chatRepository;
    }

    public async Task Delete(string id, CancellationToken cancellationToken)
    {
        await _chatRepository.Delete(id, cancellationToken);
    }
}