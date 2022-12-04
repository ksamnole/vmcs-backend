using System.Collections.Generic;
using VMCS.API.Controllers.Messages.Dto;

namespace VMCS.API.Controllers.Chats.Dto;

public class ShortChatDto
{
    public string Id { get; set; }
    public IEnumerable<ShortMessageDto> Messages { get; set; }
}