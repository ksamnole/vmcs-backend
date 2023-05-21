namespace VMCS.Core.Domains.CodeSharing.Models;

public class Change
{
    public int Position { get; set; }
    public int Action { get; set; }
    public string InsertedString { get; set; }
    public int CharsDeleted { get; set; }
    public int VersionId { get; set; }
    public int ChangeId { get; set; }
    public string ConnectionId { get; set; }
}