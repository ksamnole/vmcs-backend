using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMCS.Core.Domains.Messages.Services
{
    internal interface IMessageService
    {
        Task Create(Message message, CancellationToken token);
        Task Delete(Message message, CancellationToken token);
        Task Update(Message message, CancellationToken token);
        Task<IEnumerable<Message>> GetAllMessagesByChatId(string chatId, CancellationToken token);
        Task CreateAll(IEnumerable<Message> messages);
    }
}
