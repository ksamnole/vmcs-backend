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
    void CreateFile(int folderId, TextFile textFile);
    void DeleteFile(int fileId);
    Task Save(IDirectoryService directoryService);
    ChangeInfo ChangeFile(string text, int fileId);
}