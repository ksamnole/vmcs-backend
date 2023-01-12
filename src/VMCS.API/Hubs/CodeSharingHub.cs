using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.SignalR;
using System.Linq;
using System.Runtime.InteropServices;
using VMCS.Core.Domains.Meetings.Services;
using System.Threading;

namespace VMCS.API.Hubs
{
    public class CodeSharingHub : Hub
    { 
        private static Dictionary<string, Dictionary<int, TextFile>> _files = new Dictionary<string, Dictionary<int, TextFile>>();
        private static Dictionary<string, Dictionary<int, Folder>> _folders = new Dictionary<string, Dictionary<int, Folder>>();
        private static Dictionary<string, Repository> _repositories = new Dictionary<string, Repository>();
        private static Dictionary<string, List<string>> _reposOfConnections = new Dictionary<string, List<string>>();
        private static Dictionary<string, string> _meetingToRepositoryIds = new Dictionary<string, string>();
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMeetingService _meetingService;

        public CodeSharingHub(IWebHostEnvironment webHostEnvironment, IMeetingService meetingService)
        {
            _webHostEnvironment = webHostEnvironment;
            _meetingService = meetingService;
        }

        public override Task OnConnectedAsync()
        {
            _reposOfConnections.Add(Context.ConnectionId, new List<string>());
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            _reposOfConnections.Remove(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }

        public async Task Upload(TextFile file, int folderId, string repoId)
        {
            if (!_reposOfConnections[Context.ConnectionId].Contains(repoId))
                throw new Exception("Uploading file to not connected repository");

            var files = _folders[repoId][folderId].Files;
            files.Add(file);

            _files[repoId].Add(file.Id, file);

            await Clients.Group(repoId).SendAsync("Upload", file, folderId, repoId);
        }

        public async Task ConnectToRepository(string meetingId)
        {
            var repoId = _meetingToRepositoryIds[meetingId];
            _reposOfConnections[Context.ConnectionId].Add(repoId);
            await Groups.AddToGroupAsync(Context.ConnectionId, repoId);
            await Clients.Caller.SendAsync("ConnectToRepository", _repositories[repoId]);
        }

        public async Task CreateRepository(string meetingId, string repoName)
        {

            var entity = _repositories.Values.FirstOrDefault(r => r.MeetingId == meetingId);

            if (entity is not null)
                throw new ArgumentException($"Repository in meeting with id: {meetingId} already exists");

            var repository = new Repository()
            {
                MeetingId = meetingId,
                Name = repoName,
                Directory = new Folder()
                {
                    Id = 0,
                    Name = repoName
                }
            };

            CancellationToken cancellationToken = new CancellationTokenSource().Token;
            await _meetingService.SetRepositoryToMeeting(repository.Id, meetingId, cancellationToken);

            _reposOfConnections[Context.ConnectionId].Add(repository.Id);

            _files.Add(repository.Id, new Dictionary<int, TextFile>());
            _folders.Add(repository.Id, new Dictionary<int, Folder>() { [repository.Directory.Id] = repository.Directory });

            _repositories.Add(repository.Id, repository);

            await Groups.AddToGroupAsync(Context.ConnectionId, repository.Id);
            await Clients.Caller.SendAsync("ConnectToRepository", repository);
        }

        public async Task SaveRepository(string repoId)
        {
            var repoName = _repositories[repoId].Name;
            var repoPath = Path.Combine(_webHostEnvironment.ContentRootPath, repoName);

            var folder = new DirectoryInfo(_webHostEnvironment.ContentRootPath);
            WriteFolder(_repositories[repoId].Directory, folder);

            ZipFile.CreateFromDirectory(
                repoPath,
                Path.Combine(_webHostEnvironment.ContentRootPath, $"{repoName}.zip"));

            Directory.Delete(repoPath);
        }

        private void WriteFolder(Folder folder, DirectoryInfo currentFolder)
        {
            currentFolder = currentFolder.CreateSubdirectory(folder.Name);
            currentFolder.Create();
            foreach (var file in folder.Files)
            {
                var text = new string(file.TextInBytes.Select(b => (char)b).ToArray());
                File.WriteAllText(Path.Combine(currentFolder.FullName, file.Name), text);
            }

            foreach (var subFolder in folder.Folders)
            {
                WriteFolder(subFolder, currentFolder);
            }
        }

        

        public async Task CreateFolder(string folderName, string repoId, int parentFolderId)
        {
            if (!_reposOfConnections[Context.ConnectionId].Contains(repoId))
                throw new Exception("Creating folder in not connected repository");

            Folder folder = new Folder()
            {
                Id = _folders[repoId].Count,
                Name = folderName
            };

            _folders[repoId].Add(folder.Id, folder);
            _folders[repoId][parentFolderId].Folders.Add(folder);

            await Clients.Group(repoId).SendAsync("CreateFolder", folderName, parentFolderId, repoId);
        }

        public async Task Change(ChangeInfo change)
        {
            if (!_reposOfConnections[Context.ConnectionId].Contains(change.RepoId))
                throw new Exception("Changing files in not connected repository");

            var chint = change.InsertedChars;
            var text = _files[change.RepoId][change.FileId].TextInBytes;

            if (change.Action == ActionEnum.Insert)
            {
                text = text.Take(change.Position).Concat(chint).Concat(text.Skip(change.Position)).ToList();
            }
            if (change.Action == ActionEnum.Delete)
            {
                text = text.Take(change.Position).Concat(text.Skip(change.Position + change.CharsDeleted)).ToList();
            }
            await Clients.OthersInGroup(change.RepoId).SendAsync("Change", change);
        }
    }

    public class ChangeInfo
    {
        public string RepoId { get; set; }
        public int FileId { get; set; }
        public int Position { get; set; }
        public ActionEnum Action { get; set; }
        public byte[] InsertedChars { get; set; }
        public int CharsDeleted { get; set; }
    }

    public class Repository
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string MeetingId { get; set; }
        public string Name { get; set; }
        public Folder Directory { get; set; }
    }

    public class Folder
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<TextFile> Files { get; set; } = new List<TextFile>();
        public List<Folder> Folders { get; set; } = new List<Folder>();
    }

    public class TextFile
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<byte> TextInBytes { get; set; }
    }

    public enum ActionEnum
    {
        Insert,
        Delete
    }
}
