using System.Collections.Generic;
using VMCS.Core.Domains.CodeSharing.Models;

namespace VMCS.API.Hubs.CodeSharing
{
    public class FolderReturnDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<TextFile> Files { get; set; } = new List<TextFile>();
        public List<Folder> Folders { get; set; } = new List<Folder>();
    }
}
