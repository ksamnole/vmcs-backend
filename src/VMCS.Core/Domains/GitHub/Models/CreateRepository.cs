namespace VMCS.Core.Domains.GitHub.Models;

public class CreateRepository
{
    public string Name { get; set; }
    public bool IsPrivate { get; set; }
    public string UserId { get; set; }
    public string DirectoryId { get; set; }
}