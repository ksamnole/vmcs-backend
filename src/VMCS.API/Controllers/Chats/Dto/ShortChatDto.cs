using System.Collections.Generic;
using VMCS.API.Controllers.Messages.Dto;

namespace VMCS.API.Controllers.Chats.Dto;

public class ShortChatDto
{
    public string Id { get; set; }
    public IEnumerable<MessageDto> Messages { get; set; }
}