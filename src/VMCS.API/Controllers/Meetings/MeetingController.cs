﻿#nullable enable
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VMCS.API.Controllers.Meetings.Dto;
using VMCS.Core.Domains.Meetings.Services;
using VMCS.Core.Domains.Meetings;
using System.Threading;
using AutoMapper;
using VMCS.API.Controllers.Chats.Dto;
using VMCS.API.Controllers.Users.Dto;
using VMCS.Core;
using VMCS.Core.Domains.Chats;

namespace VMCS.API.Controllers.Meetings
{

    [ApiController]
    [Route("meeting")]
    public class MeetingController : ControllerBase
    {
        private readonly IMeetingService _meetingService;
        private readonly IMapper _mapper;

        public MeetingController(IMeetingService meetingService, IMapper mapper)
        {
            _meetingService = meetingService;
            _mapper = mapper;
        }
        
        [HttpGet("{id}")]
        public async Task<MeetingDto> GetById(string id, CancellationToken token)
        {
            var meeting = await _meetingService.GetMeetingByIdAsync(id, token);

            return new MeetingDto
            {
                Id = meeting.Id,
                Name = meeting.Name,
                Chat = _mapper.Map<ShortChatDto>(meeting.Chat),
                Users = meeting.Users.Select(x => _mapper.Map<ShortUserDto>(x)),
                RepositoryId = meeting.RepositoryId
            };
        }

        [HttpPost]
        public async Task<ShortMeetingDto> Create(CreateMeetingDto meetingDto, CancellationToken token)
        {
            var creatorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (string.IsNullOrEmpty(creatorId))
                throw new ValidationException("Please log in");
            
            var meeting = await _meetingService.Create(new Meeting()
            {
                Name = meetingDto.Name,
                IsInChannel = meetingDto.IsInChannel,
                ChannelId = meetingDto.ChannelId,
                CreatorId = creatorId
            }, token);

            return _mapper.Map<ShortMeetingDto>(meeting);
        }

        [HttpDelete("{id}")]
        public async Task Delete(string id, CancellationToken token)
        {
            await _meetingService.Delete(id, token);
        }
    }
}
