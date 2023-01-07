using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VMCS.API.Controllers.ChannelInvitations.Dto;
using VMCS.API.Controllers.Channels.Dto;
using VMCS.API.Controllers.Users.Dto;
using VMCS.Core;
using VMCS.Core.Domains.Users;
using VMCS.Core.Domains.Users.Services;

namespace VMCS.API.Controllers.Users
{
    [ApiController]
    [Route("user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<UserDto> Get(string id, CancellationToken cancellationToken)
        {
            var model = await _userService.GetById(id, cancellationToken);

            return new UserDto
            {
                Id = model.Id,
                Login = model.Login,
                Username = model.Username,
                Email = model.Email
            };
        }

        [HttpGet]
        public async Task<IEnumerable<UserDto>> GetAll(CancellationToken cancellationToken)
        {
            var users = await _userService.GetAll(cancellationToken);

            return users.Select(it => new UserDto
                {
                    Id = it.Id,
                    Login = it.Login,
                    Username = it.Username,
                    Email = it.Email
                    
                });
        }
        
        [HttpGet]
        [Route("channels")]
        public async Task<IEnumerable<ShortChannelDto>> GetAllUserChannels(CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (string.IsNullOrEmpty(userId))
                throw new ValidationException("Please log in");

            var channels = await _userService.GetAllUserChannels(userId, cancellationToken);

            return channels.Select(x => new ShortChannelDto()
            {
                Id = x.Id,
                Name = x.Name,
                ChatId = x.ChatId
            });
        }
        
        [HttpGet]
        [Route("invitations/channel")]
        public async Task<IEnumerable<ChannelInvitationDto>> GetAllUserChannelInvitations(CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (string.IsNullOrEmpty(userId))
                throw new ValidationException("Please log in");

            var channelInvitations = await _userService.GetAllUserChannelInvitations(userId, cancellationToken);
            return channelInvitations.Select(x => new ChannelInvitationDto()
            {
                SenderUsername = x.Sender.Username,
                RecipientUsername = x.Recipient.Username,
                ChannelName = x.Channel.Name,
                Id = x.Id
            });
        }

        [HttpPut("{id}")]
        public async Task Update(string id, ChangeUserDto model, CancellationToken cancellationToken)
        {
            await _userService.Update(new User
            {
                Id = id,
                Username = model.Username,
                Email = model.Email
            }, cancellationToken);
        }

        [HttpDelete("{id}")]
        public async Task Delete(string id, CancellationToken cancellationToken)
        {
            await _userService.Delete(id, cancellationToken);
        }
    }
}
