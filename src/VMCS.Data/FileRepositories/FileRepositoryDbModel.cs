namespace VMCS.Data.FileRepositories;

public class FileRepositoryDbModel
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string DirectoryInJson { get; set; }
    public byte[] DirectoryZip { get; set; }
}