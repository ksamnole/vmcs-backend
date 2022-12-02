using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VMCS.API.Controllers.Meetings.Dto;
using VMCS.Core.Domains.Meetings.Services;
using VMCS.Core.Domains.Meetings;
using System.Threading;

namespace VMCS.API.Controllers.Meetings
{

    [ApiController]
    [Route("meeting")]
    public class MeetingController : ControllerBase
    {
        private IMeetingService _meetingService;

        public MeetingController(IMeetingService meetingService)
        {
            _meetingService = meetingService;
        }

        [HttpPost]
        public async Task Create(MeetingDto meetingDto, CancellationToken token)
        {
            await _meetingService.Create(new Meeting()
            {
                Name = meetingDto.Name,
                IsInChannel = meetingDto.IsInChannel,
                ChannelId = meetingDto.ChannelId,
                UserId = meetingDto.UserId
            }, token);
        }

        [HttpDelete("{id}")]
        public async Task Delete(string id, CancellationToken token)
        {
            await _meetingService.Delete(id, token);
        }

        [HttpGet("{id}")]
        public async Task<MeetingDto> GetById(string id, CancellationToken token)
        {
            var meeting = await _meetingService.GetMeetingByIdAsync(id, token);

            return new MeetingDto
            {
                Name = meeting.Name,
                IsInChannel = meeting.IsInChannel,
                ChannelId = meeting.ChannelId,
                UserId = meeting.UserId
            };
        }
    }
}
