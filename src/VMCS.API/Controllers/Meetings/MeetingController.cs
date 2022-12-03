using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VMCS.API.Controllers.Meetings.Dto;
using VMCS.Core.Domains.Meetings.Services;
using VMCS.Core.Domains.Meetings;
using System.Threading;
using VMCS.Core;
using VMCS.Core.Domains.Chats;

namespace VMCS.API.Controllers.Meetings
{

    [ApiController]
    [Route("meeting")]
    public class MeetingController : ControllerBase
    {
        private readonly IMeetingService _meetingService;

        public MeetingController(IMeetingService meetingService)
        {
            _meetingService = meetingService;
        }
        
        [HttpGet("{id}")]
        public async Task<MeetingDto> GetById(string id, CancellationToken token)
        {
            var meeting = await _meetingService.GetMeetingByIdAsync(id, token);

            return new MeetingDto
            {
                Id = meeting.Id,
                Name = meeting.Name,
                Chat = meeting.Chat,
                Users = meeting.Users
            };
        }

        [HttpPost]
        public async Task Create(CreateMeetingDto meetingDto, CancellationToken token)
        {
            var creatorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (string.IsNullOrEmpty(creatorId))
                throw new ValidationException("Please log in");
            
            await _meetingService.Create(new Meeting()
            {
                Name = meetingDto.Name,
                IsInChannel = meetingDto.IsInChannel,
                ChannelId = meetingDto.ChannelId,
                CreatorId = creatorId,
                Chat = new Chat()
            }, token);
        }

        [HttpDelete("{id}")]
        public async Task Delete(string id, CancellationToken token)
        {
            await _meetingService.Delete(id, token);
        }
    }
}
