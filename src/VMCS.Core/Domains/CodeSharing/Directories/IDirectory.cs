using VMCS.Core.Domains.CodeSharing.Models;
using VMCS.Core.Domains.Directories.Services;

namespace VMCS.Core.Domains.CodeSharing.Directories;

public interface IDirectory
{
    string MeetingId { get; }
    string Name { get; }
    string Id { get; }
    Folder RootFolder { get; }

    Folder CreateFolder(Folder folder, int parentFolderId);
    void DeleteFolder(int folderId);
    void CreateFile(TextFile textFile, int folderId);
    void DeleteFile(int fileId);
    Task Save(IDirectoryService directoryService);
    void ChangeFile(int fileId, string text);
}