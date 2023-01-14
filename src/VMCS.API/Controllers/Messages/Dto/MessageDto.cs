using System;

namespace VMCS.API.Controllers.Messages.Dto;

public class MessageDto
{
    public string Id { get; set; }
    public string UserId { get; set; }
    public string ChatId { get; set; }
    public string Username { get; set; }
    public string Text { get; set; }
    public DateTime ModifiedAt { get; set; }
}