using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VMCS.API.Controllers.Channel.Dto;
using VMCS.API.Controllers.Channels.Dto;
using VMCS.API.Controllers.Chats.Dto;
using VMCS.API.Controllers.Meetings.Dto;
using VMCS.API.Controllers.Users.Dto;
using VMCS.Core;
using VMCS.Core.Domains.Channels.Services;

namespace VMCS.API.Controllers.Channels;

[ApiController]
[Route("channels")]
[Authorize]
public class ChannelController : ControllerBase
{
    private readonly IChannelService _channelService;
    private readonly IMapper _mapper;

    public ChannelController(IChannelService channelService, IMapper mapper)
    {
        _channelService = channelService;
        _mapper = mapper;
    }

    [HttpGet("{id}")]
    public async Task<ChannelDto> Get(string id, CancellationToken cancellationToken)
    {
        var model = await _channelService.GetById(id, cancellationToken);

        return new ChannelDto
        {
            Id = model.Id,
            Name = model.Name,
            Chat = _mapper.Map<ShortChatDto>(model.Chat),
            Creator = _mapper.Map<ShortUserDto>(model.Creator),
            Users = model.Users.Select(x => _mapper.Map<ShortUserDto>(x)),
            Meetings = model.Meetings.Select(x => _mapper.Map<ShortMeetingDto>(x))
        };
    }

    [HttpPost]
    public async Task<ShortChannelDto> Create(CreateChannelDto model, CancellationToken cancellationToken)
    {
        var creatorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(creatorId))
            throw new ValidationException("Please log in");

        var channel = await _channelService.Create(new Core.Domains.Channels.Channel
        {
            Name = model.Name,
            CreatorId = creatorId
        }, cancellationToken);

        return _mapper.Map<ShortChannelDto>(channel);
    }

    [HttpDelete("{id}")]
    public async Task Delete(string id, CancellationToken cancellationToken)
    {
        await _channelService.Delete(id, cancellationToken);
    }
}