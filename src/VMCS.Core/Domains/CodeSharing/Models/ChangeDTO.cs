namespace VMCS.Core.Domains.CodeSharing.Models;

public class ChangeDTO
{
    public string DirectoryId { get; set; }
    public int FileId { get; set; }
    public Change Change { get; set; }
    public string ConnectionId { get; set; }
}