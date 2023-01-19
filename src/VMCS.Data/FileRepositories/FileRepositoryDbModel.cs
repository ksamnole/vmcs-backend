using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace VMCS.Data.FileRepositories
{
    public class FileRepositoryDbModel
    {
        public static string Id { get; set; }
        public static string Name { get; set; }
        public static string DirectoryInJson { get; set; }
        public static byte[] DirectoryZip { get; set; }
    }
}
