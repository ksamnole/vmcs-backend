namespace VMCS.Core.Domains.CodeSharing.Models;

public class TextFile
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Text { get; set; }
    public bool IsDeleted { get; set; }
}