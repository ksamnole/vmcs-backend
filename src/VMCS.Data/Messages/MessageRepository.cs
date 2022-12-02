
using Microsoft.EntityFrameworkCore;
using System.Data.Entity.Core;
using VMCS.Core.Domains.Messages;
using VMCS.Core.Domains.Messages.Repositories;
using VMCS.Data.Contexts;

namespace VMCS.Data.Messages
{
    public class MessageRepository : IMessageRepository
    {
        private readonly ApplicationContext _applicationContext;

        public MessageRepository(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }


        public async Task Create(Message message, CancellationToken token)
        {
            await _applicationContext.Messages.AddAsync(new MessageDbModel(message));
        }

        public async Task CreateAll(IEnumerable<Message> messages, CancellationToken token)
        {
            foreach(var message in messages)
            {
                await Create(message, token);
            }
        }

        public async Task Delete(string id, CancellationToken token)
        {
            var message = await _applicationContext.Messages.FirstOrDefaultAsync(m => m.Id == id);

            if (message is null)
            {
                throw new ObjectNotFoundException($"Message with id = {id} not found");
            }

            _applicationContext.Messages.Remove(message);
        }

        public async Task<IEnumerable<Message>> GetAllMessagesByChatId(string chatId, CancellationToken token)
        {
            return await _applicationContext.Messages
            .Where(m => m.ChatId == chatId)
            .Select(m => new Message()
            {
                Chat = m.Chat,
                Id = m.Id,
                ChatId= m.ChatId,
                User = m.User,
                UserId = m.UserId,
                CreatedAt = m.CreatedAt,
                Text= m.Text
            })
            .ToArrayAsync(token);
        }

        public async Task<Message> GetById(string id, CancellationToken token)
        {
            var message = await _applicationContext.Messages.FirstOrDefaultAsync(m => m.Id == id);

            if (message is null)
            {
                throw new ObjectNotFoundException($"Message with id = {id} not found");
            }

            return new Message()
            {
                Chat = message.Chat,
                Id = message.Id,
                ChatId = message.ChatId,
                User = message.User,
                UserId = message.UserId,
                CreatedAt = message.CreatedAt,
                Text = message.Text
            };
        }

        public async Task Update(Message message, CancellationToken token)
        {
            var entity = await _applicationContext.Messages.FirstOrDefaultAsync(m => m.Id == message.Id);

            if (entity is null)
            {
                throw new ObjectNotFoundException($"Message with id = {message.Id} not found");
            }

            entity.Text = message.Text;
        }
    }
}
