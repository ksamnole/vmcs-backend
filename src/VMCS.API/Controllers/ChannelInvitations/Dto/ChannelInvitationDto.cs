namespace VMCS.API.Controllers.ChannelInvitations.Dto;

public class ChannelInvitationDto
{
    public string Id { get; set; }
    public string SenderUsername { get; set; }
    public string RecipientUsername { get; set; }
    public string ChannelName { get; set; }
}