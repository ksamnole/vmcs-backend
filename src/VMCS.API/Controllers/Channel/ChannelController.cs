using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VMCS.API.Controllers.Channel.Dto;
using VMCS.API.Controllers.Meetings.Dto;
using VMCS.Core.Domains.Channels.Services;
using VMCS.Core.Domains.Chats;

namespace VMCS.API.Controllers.Channel
{
    [ApiController]
    [Route("channels")]
    public class ChannelController : ControllerBase
    {
        private readonly IChannelService _channelService;

        public ChannelController(IChannelService channelService)
        {
            _channelService = channelService;
        }

        [HttpGet("{id}")]
        public async Task<ChannelDto> Get(string id, CancellationToken cancellationToken)
        {
            var model = await _channelService.GetById(id, cancellationToken);

            return new ChannelDto()
            {
                Id = model.Id,
                Name = model.Name,
                Chat = model.Chat,
                Users = model.Users,
                Meetings = model.Meetings.Select(x => new ShortMeetingDto()
                {
                    Id = x.Id,
                    Name = x.Name
                })
            };
        }

        [HttpPost]
        public async Task Create(CreateChannelDto model, CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await _channelService.Create(new Core.Domains.Channels.Channel()
            {
                Name = model.Name,
                CreatorId = userId,
                Chat = new Chat()
            }, cancellationToken);
        }
        
        [HttpDelete("{id}")]
        public async Task Delete(string id, CancellationToken cancellationToken)
        {
            await _channelService.Delete(id, cancellationToken);
        }
    }
}