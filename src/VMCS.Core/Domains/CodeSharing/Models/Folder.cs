using System.Text.Json.Serialization;

namespace VMCS.Core.Domains.CodeSharing.Models;

public class Folder
{
    public Folder() { }

    public Folder(string name)
    {
        Name = name;
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public List<TextFile> Files { get; set; } = new();
    public List<Folder> Folders { get; set; } = new();

    [JsonIgnore] public bool IsDeleted { get; set; }

    public void DeleteDeletedObjects()
    {
        // Delete deleted files
        Files.RemoveAll(f => f.IsDeleted);

        // Delete deleted folders
        Folders.RemoveAll(f => f.IsDeleted);

        // Recursively go through each folder and delete deleted objects
        foreach (var folder in Folders) folder.DeleteDeletedObjects();
    }
}