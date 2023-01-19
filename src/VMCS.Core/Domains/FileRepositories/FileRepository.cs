using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMCS.Core.Domains.CodeSharing.Models;

namespace VMCS.Core.Domains.FileRepositories
{
    public class FileRepository : IFileRepository
    {
        public string Id { get; } = Guid.NewGuid().ToString();
        public string MeetingId { get; }
        public string Name { get; }
        public Folder Directory { get; }

        private Dictionary<int, TextFile> _repositoryFiles = new Dictionary<int, TextFile>();
        private Dictionary<int, Folder> _repositoryFolders = new Dictionary<int, Folder>();

        public FileRepository(string meetingId, string name)
        {
            MeetingId = meetingId;
            Name = name;
            Directory = new Folder()
            {
                Id = 0,
                Files = new List<TextFile>(),
                Folders = new List<Folder>(),
                Name = name
            };

            _repositoryFolders.Add(0, Directory);
        }

        public void CreateFolder(string folderName, int parentFolderId)
        {
            var folder = new Folder() 
            { 
                Id = _repositoryFolders.Count, 
                Files = new List<TextFile>(),
                Folders = new List<Folder>(),
                Name = folderName
            };

            if (!_repositoryFolders.ContainsKey(parentFolderId))
                throw new ArgumentException($"Folder with id {parentFolderId} doesn`t exist");

            _repositoryFolders.Add(folder.Id, folder);      
            _repositoryFolders[parentFolderId] = folder;
        }

        public void DeleteFolder(int folderId)
        {
            if (!_repositoryFolders.ContainsKey(folderId) || _repositoryFolders[folderId].IsDeleted)
                throw new ArgumentException($"Folder with id {folderId} doesn`t exist");

            _repositoryFolders[folderId].IsDeleted = true;
        }

        public void DeleteFile(int fileId)
        {
            if (!_repositoryFiles.ContainsKey(fileId) || _repositoryFiles[fileId].IsDeleted)
                throw new ArgumentException($"File with id {fileId} doesn't exist");

            _repositoryFiles[fileId].IsDeleted = true;
        }

        public void UploadFile(int folderId, TextFile file)
        {
            var files = _repositoryFolders[folderId].Files;
            files.Add(file);

            _repositoryFiles.Add(file.Id, file);
        }

        public async Task Save()
        {
            var path = Path.GetTempPath();
            var repoPath = Path.Combine(path, Name);

            var folder = new DirectoryInfo(path);
            WriteFolder(Directory, folder);

            ZipFile.CreateFromDirectory(
                path,
                Path.Combine(path, $"{Name}.zip"));

            System.IO.Directory.Delete(repoPath);
        }

        private void WriteFolder(Folder folder, DirectoryInfo currentFolder)
        {
            currentFolder = currentFolder.CreateSubdirectory(folder.Name);
            currentFolder.Create();
            //foreach (var file in folder.Files)
            //{
            //    var text = new string(file.TextInBytes.Select(b => (char)b).ToArray());
            //    File.WriteAllText(Path.Combine(currentFolder.FullName, file.Name), text);
            //}

            foreach (var subFolder in folder.Folders)
            {
                WriteFolder(subFolder, currentFolder);
            }
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public void ChangeFile(string text, int fileId)
        {
            if (!_repositoryFiles.ContainsKey(fileId))
                throw new ArgumentException($"No file with id {fileId}");

            _repositoryFiles[fileId].Text = text;
        }
    }
}
