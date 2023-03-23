using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VMCS.API.Controllers.ChannelInvitations.Dto;
using VMCS.API.Controllers.Channels.Dto;
using VMCS.API.Controllers.Users.Dto;
using VMCS.Core;
using VMCS.Core.Domains.Auth;
using VMCS.Core.Domains.Users;
using VMCS.Core.Domains.Users.Services;

namespace VMCS.API.Controllers.Users;

[ApiController]
[Route("user")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IWebHostEnvironment _webHostEnv;

    public UserController(IUserService userService, IWebHostEnvironment env, UserManager<AuthUser> userManager)
    {
        _userService = userService;
        _webHostEnv = env;
    }

    [HttpGet]
    public async Task<UserDto> Get(CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            throw new ValidationException("Please log in");

        var model = await _userService.GetById(userId, cancellationToken);

        return new UserDto
        {
            Id = model.Id,
            Login = model.Login,
            Username = model.Username,
            Email = model.Email,
            AvatarUri = model.AvatarUri
        };
    }

    [HttpGet]
    [Route("all")]
    public async Task<IEnumerable<UserDto>> GetAll(CancellationToken cancellationToken)
    {
        var users = await _userService.GetAll(cancellationToken);

        return users.Select(it => new UserDto
        {
            Id = it.Id,
            Login = it.Login,
            Username = it.Username,
            Email = it.Email,
            AvatarUri = it.AvatarUri
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

        return channels.Select(x => new ShortChannelDto
        {
            Id = x.Id,
            Name = x.Name,
            ChatId = x.ChatId,
            AvatarUri = x.AvatarUri
        });
    }

    [HttpGet]
    [Route("invitations/channel")]
    public async Task<IEnumerable<ChannelInvitationDto>> GetAllUserChannelInvitations(
        CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            throw new ValidationException("Please log in");

        var channelInvitations = await _userService.GetAllUserChannelInvitations(userId, cancellationToken);
        return channelInvitations.Select(x => new ChannelInvitationDto
        {
            SenderUsername = x.Sender.Username,
            RecipientUsername = x.Recipient.Username,
            ChannelName = x.Channel.Name,
            Id = x.Id
        });
    }

    [HttpGet]
    [Route("access-token")]
    public async Task<bool> IsUserHaveAccessToken()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            throw new ValidationException("Please log in");
        
        return await _userService.IsUserHaveAccessToken(userId);
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

    [HttpPost]
    [Route("upload-avatar")]
    public async Task UploadAvatar(IFormFile image, CancellationToken cancellationToken)
    {
        byte[] bytes;
        using (var stream = new MemoryStream())
        {
            await image.CopyToAsync(stream, cancellationToken);
            bytes = stream.ToArray();
        }

        var name = Guid.NewGuid() + "." + image.FileName.Split(".")[^1];
        var avatarUrl = $"/imgs/{name}";

        var savePath = Path.Combine(_webHostEnv.WebRootPath, "imgs", name);

        await System.IO.File.WriteAllBytesAsync(savePath, bytes.ToArray(), cancellationToken);

        await _userService.SetAvatarImage(User.FindFirstValue(ClaimTypes.NameIdentifier), avatarUrl, cancellationToken);
    }
}