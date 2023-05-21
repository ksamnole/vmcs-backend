using Microsoft.AspNetCore.Http;

namespace VMCS.API.Controllers.Channels.Dto;

public class UploadAvatarChannelDTO
{
    public string ChannelId { get; set; }
    public IFormFile Image { get; set; }
}