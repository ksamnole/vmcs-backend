using System.Collections.Generic;
using VMCS.Core.Domains.CodeSharing.Models;

namespace VMCS.API.Hubs.CodeSharing.Dto;

public class FolderReturnDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<TextFile> Files { get; set; }
    public List<Folder> Folders { get; set; }
    public int ParentId { get; set; }
}