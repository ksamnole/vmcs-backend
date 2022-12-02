using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMCS.Core.Domains.Chats;
using VMCS.Core.Domains.Messages;
using VMCS.Core.Domains.Messages.Repositories;
using VMCS.Core.Domains.Users;
using VMCS.Data.Contexts;

namespace VMCS.Data.Messages
{
    public class MessageDbModel
    {     
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string UserId { get; set; }
        public User User { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string ChatId { get; set; }
        public Chat Chat { get; set; }

        public MessageDbModel(Message message)
        {
            Id        = message.Id;
            UserId    = message.UserId;
            User      = message.User;
            Text      = message.Text;
            CreatedAt = message.CreatedAt;
            ChatId    = message.ChatId;
            Chat      = message.Chat;
        }
    }
}
