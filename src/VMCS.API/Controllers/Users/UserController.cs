using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VMCS.API.Controllers.Users.Dto;
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
