using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMCS.Core.Extensions
{
    public static class ZipArchiveExtensions
    {
        public static ZipArchive AddTextFile(this ZipArchive zipArchive, string fileName, string text)
        {
            var entry = zipArchive.CreateEntry(fileName);

            using var stream = entry.Open();
            using var writer = new StreamWriter(stream);

            writer.Write(text);

            return zipArchive;
        }
    }
}
