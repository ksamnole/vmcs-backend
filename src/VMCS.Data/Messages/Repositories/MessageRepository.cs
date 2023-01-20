using System.Data.Entity.Core;
using Microsoft.EntityFrameworkCore;
using VMCS.Core.Domains.Messages;
using VMCS.Core.Domains.Messages.Repositories;
using VMCS.Data.Contexts;

namespace VMCS.Data.Messages.Repositories;

public class MessageRepository : IMessageRepository
{
    private readonly ApplicationContext _applicationContext;

    public MessageRepository(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }


    public async Task Create(Message message, CancellationToken token)
    {
        await _applicationContext.Messages.AddAsync(message, token);
    }

    public async Task CreateAll(IEnumerable<Message> messages, CancellationToken token)
    {
        foreach (var message in messages) await Create(message, token);
    }

    public async Task Delete(string id, CancellationToken token)
    {
        var message = await _applicationContext.Messages.FirstOrDefaultAsync(m => m.Id == id, token);

        if (message is null)
            throw new ObjectNotFoundException($"Message with id = {id} not found");

        _applicationContext.Messages.Remove(message);
    }

    public async Task<IEnumerable<Message>> GetAllMessagesByChatId(string chatId, CancellationToken token)
    {
        return await _applicationContext.Messages.Where(m => m.Chat.Id == chatId).ToArrayAsync(token);
    }

    public async Task<Message> GetById(string id, CancellationToken token)
    {
        var message = await _applicationContext.Messages.FirstOrDefaultAsync(m => m.Id == id, token);

        if (message is null)
            throw new ObjectNotFoundException($"Message with id = {id} not found");

        return message;
    }

    public async Task Update(Message message, CancellationToken token)
    {
        var entity = await _applicationContext.Messages.FirstOrDefaultAsync(m => m.Id == message.Id, token);

        if (entity is null) throw new ObjectNotFoundException($"Message with id = {message.Id} not found");

        entity.Text = message.Text;
    }
}