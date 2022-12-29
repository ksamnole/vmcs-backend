using VMCS.Core.Domains.Messages.Repositories;

namespace VMCS.Core.Domains.Messages.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IUnitOfWork _unitOfWork;

        public MessageService(IMessageRepository messageRepository, IUnitOfWork unitOfWork)
        {
            _messageRepository = messageRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Message> Create(Message message, CancellationToken token)
        {
            await _messageRepository.Create(message, token);
            await _unitOfWork.SaveChange();
            
            return message;
        }

        public async Task CreateAll(IEnumerable<Message> messages, CancellationToken token)
        {
            await _messageRepository.CreateAll(messages, token);
            await _unitOfWork.SaveChange();
        }

        public async Task Delete(string id, CancellationToken token)
        {
            await _messageRepository.Delete(id, token);
            await _unitOfWork.SaveChange();
        }

        public async Task<IEnumerable<Message>> GetAllMessagesByChatId(string chatId, CancellationToken token)
        {
            return await _messageRepository.GetAllMessagesByChatId(chatId, token); 
        }

        public async Task<Message> GetById(string id, CancellationToken token)
        {
            return await _messageRepository.GetById(id, token);
        }

        public async Task Update(Message message, CancellationToken token)
        {
            await _messageRepository.Update(message, token);
            await _unitOfWork.SaveChange();
        }
    }
}
