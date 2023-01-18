using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMCS.Core.Domains.CodeSharing.Models
{
    public class Folder
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<TextFile> Files { get; set; } = new List<TextFile>();
        public List<Folder> Folders { get; set; } = new List<Folder>();
        public bool IsDeleted { get; set; }
    }
}
