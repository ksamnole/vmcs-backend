namespace VMCS.Core.Domains.CodeSharing.Models;

public class ChangeInfo
{
    public string RepoId { get; set; }
    public string FileId { get; set; }
    public int Position { get; set; }
    public ActionEnum Action { get; set; }
    public byte[] InsertedChars { get; set; }
    public int CharsDeleted { get; set; }
}