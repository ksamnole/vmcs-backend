using VMCS.Core.Domains.CodeSharing.Models;
using VMCS.Core.Domains.FileRepositories;
using VMCS.Core.Domains.Meetings.Services;

namespace VMCS.Core.Domains.CodeSharing;

public interface ICodeSharing
{
    void Upload(TextFile file, int folderId, string repsitoryoId, string connectionId);

    Task<FileRepository> CreateRepository(string meetingId, string repositoryName, string connectionId,
        IMeetingService meetingService);

    void ConnectToRepository(string repositoryId, string connectionId);
    Task SaveRepository(string repositoryId);
    Folder CreateFolder(Folder folder, string repositoryId, int parentFolderId, string connectionId);
    void Change(string text, string repositoryId, int fileId, string connectionId);
    void AddConnection(string connectionId);
    void RemoveConnection(string connectionId);
    Task<IFileRepository> GetRepositoryById(string repositoryId);
}