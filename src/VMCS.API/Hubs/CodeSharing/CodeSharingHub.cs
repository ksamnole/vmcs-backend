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
using VMCS.Core.Domains.CodeSharing.Models;
using VMCS.Core.Domains.CodeSharing;

namespace VMCS.API.Hubs.CodeSharing
{
    public class CodeSharingHub : Hub
    {
        private static ICodeSharing _codeSharing;
        private static IMeetingService _meetingService;

        public CodeSharingHub(IWebHostEnvironment webHostEnvironment, IMeetingService meetingService, ICodeSharing codeSharing)
        {
            _codeSharing = codeSharing;
            _meetingService = meetingService;
        }

        public override Task OnConnectedAsync()
        {
            _codeSharing.AddConnection(Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            _codeSharing.RemoveConnection(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }

        public async Task Upload(TextFileDTO file, int folderId, string repoId)
        {
            var entity = new TextFile() { Name = file.Name, Text = file.Text };
            _codeSharing.Upload(entity, folderId, repoId, Context.ConnectionId);

            await Clients.Group(repoId).SendAsync("Upload", new TextFileReturnDTO { Id = entity.Id, Name = file.Name, Text = file.Text});
        }

        public async Task ConnectToRepository(string repositoryId)
        {
            _codeSharing.ConnectToRepository(repositoryId, Context.ConnectionId);

            await Groups.AddToGroupAsync(Context.ConnectionId, repositoryId);

            var repository = await _codeSharing.GetRepositoryById(repositoryId);
            await Clients.Caller.SendAsync("ConnectToRepository", repository);
        }

        public async Task CreateRepository(string meetingId, string repoName)
        {
            var repository = await _codeSharing.CreateRepository(meetingId, repoName, Context.ConnectionId, _meetingService);

            await Groups.AddToGroupAsync(Context.ConnectionId, repository.Id);
            await Clients.Caller.SendAsync("ConnectToRepository", repository);
        }

        public async Task SaveRepository(string repoId)
        {
            await _codeSharing.SaveRepository(repoId);
        }

        public async Task CreateFolder(string folderName, string repoId, int parentFolderId)
        {
            var folder = _codeSharing.CreateFolder(new Folder(folderName), repoId, parentFolderId, Context.ConnectionId);
            var returnDTO = new FolderReturnDTO() 
            { 
                Id= folder.Id, 
                Name = folder.Name, 
                Files = folder.Files, 
                Folders = folder.Folders
            };

            await Clients.Group(repoId).SendAsync("CreateFolder", returnDTO);
        }

        public async Task Change(string text, string repositoryId, int fileId)
        {
            _codeSharing.Change(text, repositoryId, fileId, Context.ConnectionId);
            //_codeSharing.Change(change, Context.ConnectionId);

            await Clients.Group(repositoryId).SendAsync("Change", text, repositoryId, fileId);
        }
    }
}
