using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMCS.Core.Domains.CodeSharing.Models;

namespace VMCS.Core.Domains.FileRepositories
{
    public interface IFileRepository
    {
        string MeetingId { get; }
        string Name { get; }
        string Id { get; }
        Folder Directory { get; }

        Folder CreateFolder(string folderName, int parentFolderId);
        void DeleteFolder(int folderId);
        void UploadFile(int folderId, TextFile textFile);
        void DeleteFile(int fileId);
        Task Save();
        void Delete();
        void ChangeFile(string text, int fileId);
    }
}
