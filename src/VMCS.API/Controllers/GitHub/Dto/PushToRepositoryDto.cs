namespace VMCS.API.Controllers.GitHub.Dto;

public class PushToRepositoryDto
{
    public string RepositoryName { get; set; }
    public string DirectoryId { get; set; }
    public string Message { get; set; }
}