namespace VMCS.API.Controllers.Meetings.Dto;

public class CreateMeetingDto
{
    public string Name { get; set; }
    public bool IsInChannel { get; set; }
    public string ChannelId { get; set; }
}