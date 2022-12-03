using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VMCS.API.Controllers.Channel.Dto;
using VMCS.Core.Domains.Channels.Services;

namespace VMCS.API.Controllers.Channel
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
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
                Name = model.Name
            };
        }


        [HttpPost]
        public async Task Create(ChannelDto model, CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _channelService.Create(new Core.Domains.Channels.Channel()
            {
                Name = model.Name,
                CreatorId = userId
            }, cancellationToken);
        }
        
        [HttpDelete("{id}")]
        public async Task Delete(string id, CancellationToken cancellationToken)
        {
            await _channelService.Delete(id, cancellationToken);
        }
    }
}