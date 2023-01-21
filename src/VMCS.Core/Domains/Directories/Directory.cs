using VMCS.Core.Domains.Meetings;

namespace VMCS.Core.Domains.Directories;

public class Directory
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; }
    public string? DirectoryInJson { get; set; }
    public byte[]? DirectoryZip { get; set; }

    public string MeetingId { get; set; }
    public Meeting Meeting { get; set; }
}