namespace VMCS.Core.Domains.Messages.Repositories;

public interface IMessageRepository
{
    Task Create(Message message, CancellationToken token);
    Task Delete(string id, CancellationToken token);
    Task Update(Message message, CancellationToken token);
    Task<IEnumerable<Message>> GetAllMessagesByChatId(string chatId, CancellationToken token);
    Task<Message> GetById(string id, CancellationToken token);
    Task CreateAll(IEnumerable<Message> messages, CancellationToken token);
}