using System.Collections.Generic;
using VMCS.API.Controllers.Chats.Dto;
using VMCS.API.Controllers.Users.Dto;

namespace VMCS.API.Controllers.Meetings.Dto
{
    public class MeetingDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public ShortChatDto Chat { get; set; }
        public IEnumerable<ShortUserDto> Users { get; set; }
        public string? RepositoryId { get; set; }
    }
}
