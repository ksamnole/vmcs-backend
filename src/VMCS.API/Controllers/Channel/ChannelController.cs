using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VMCS.API.Controllers.Channel.Dto;
using VMCS.Core.Domains.Channels.Services;

namespace VMCS.API.Controllers.Channel
{
    [ApiController]
    [Route("[controller]")]
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
                Name = model.Name
            };
        }
        
        [HttpPost]
        public async Task Create(ChannelDto model, CancellationToken cancellationToken)
        {
            await _channelService.Create(new Core.Domains.Channels.Channel()
            {
                Id = model.Id,
                Name = model.Name
            }, cancellationToken);
        }
        
        [HttpDelete("{id}")]
        public async Task Delete(string id, CancellationToken cancellationToken)
        {
            await _channelService.Delete(id, cancellationToken);
        }
    }
}