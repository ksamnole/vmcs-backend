using System.IO.Compression;

namespace VMCS.Core.Extensions;

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