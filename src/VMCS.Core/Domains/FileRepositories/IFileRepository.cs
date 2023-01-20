﻿using VMCS.Core.Domains.CodeSharing.Models;

namespace VMCS.Core.Domains.FileRepositories;

public interface IFileRepository
{
    string MeetingId { get; }
    string Name { get; }
    string Id { get; }
    Folder Directory { get; }

    Folder CreateFolder(Folder folder, int parentFolderId);
    void DeleteFolder(int folderId);
    void UploadFile(int folderId, TextFile textFile);
    void DeleteFile(int fileId);
    Task Save();
    void Delete();
    void ChangeFile(string text, int fileId);
}