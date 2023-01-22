namespace VMCS.API.Hubs.CodeSharing.Dto;

public class TextFileReturnDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Text { get; set; }
    public int ParentId { get; set; }
}