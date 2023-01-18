using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMCS.Core.Domains.CodeSharing.Models
{
    public class ChangeInfo
    {
        public string RepoId { get; set; }
        public int FileId { get; set; }
        public int Position { get; set; }
        public ActionEnum Action { get; set; }
        public byte[] InsertedChars { get; set; }
        public int CharsDeleted { get; set; }
    }
}
