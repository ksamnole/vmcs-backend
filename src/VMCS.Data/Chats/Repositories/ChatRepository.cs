using System.Data.Entity.Core;
using Microsoft.EntityFrameworkCore;
using VMCS.Core.Domains.Chats.Repositories;
using VMCS.Data.Contexts;

namespace VMCS.Data.Chats.Repositories;

public class ChatRepository : IChatRepository
{
    private readonly ApplicationContext _applicationContext;
    
    public ChatRepository(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }
    
    public async Task Delete(string id, CancellationToken cancellationToken)
    {
        var entity = await _applicationContext.Chats.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (entity is null)
            throw new ObjectNotFoundException($"Chat with id = {id} not found");

        _applicationContext.Chats.Remove(entity);
    }
}