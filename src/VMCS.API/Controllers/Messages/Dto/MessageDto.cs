using System;
using VMCS.API.Controllers.Users.Dto;

namespace VMCS.API.Controllers.Messages.Dto;

public class MessageDto
{
    public string Id { get; set; }
    public string ChatId { get; set; }
    public ShortUserDto User { get; set; }
    public string Text { get; set; }
    public DateTime ModifiedAt { get; set; }
}