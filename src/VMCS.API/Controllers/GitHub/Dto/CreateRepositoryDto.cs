namespace VMCS.API.Controllers.GitHub.Dto;

public class CreateRepositoryDto
{
    public string Name { get; set; }
    public bool IsPrivate { get; set; }
    public string UserId { get; set; }
}