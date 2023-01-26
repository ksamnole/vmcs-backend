namespace VMCS.Core.Domains.CodeSharing.Models;

public class ChangeInfo
{
    public string DirId { get; set; }
    public int FileId { get; set; }
    public int Position { get; set; }
    public ActionEnum Action { get; set; }
    public string InsertedString { get; set; }
    public int CharsDeleted { get; set; }
}