namespace VMCS.Core.Domains.CodeSharing.Models;

public class Folder
{
    public Folder()
    {
    }

    public Folder(string name)
    {
        Name = name;
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public List<TextFile> Files { get; set; } = new();
    public List<Folder> Folders { get; set; } = new();
    public bool IsDeleted { get; set; }
}