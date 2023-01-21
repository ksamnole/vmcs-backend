using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using VMCS.Core.Domains.CodeSharing;
using VMCS.Core.Domains.CodeSharing.Directories;
using VMCS.Core.Domains.CodeSharing.Models;
using VMCS.Core.Domains.Directories.Services;
using VMCS.Core.Domains.Meetings.Services;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace VMCS.API.Hubs.CodeSharing;

public class CodeSharingHub : Hub
{
    private readonly IValidator<TextFile> _fileValidator;
    private readonly IDirectoryService _directoryService;
    private static readonly Dictionary<string, IDirectory> _directories = new();
    private static readonly Dictionary<string, string> _connectionDirectory = new();

    public CodeSharingHub(IValidator<TextFile> fileValidator, IDirectoryService directoryService)
    {
        _fileValidator = fileValidator;
        _directoryService = directoryService;
    }

    public override Task OnDisconnectedAsync(Exception exception)
    {
        _connectionDirectory.Remove(Context.ConnectionId);
        return base.OnDisconnectedAsync(exception);
    }

    public async Task Upload(TextFileDTO file, int folderId, string directoryId)
    {
        var entity = new TextFile { Name = file.Name, Text = file.Text };

        if (!_connectionDirectory[Context.ConnectionId].Contains(directoryId))
            throw new Exception("Uploading file to not connected directory");

        _fileValidator.ValidateAndThrow(entity);

        _directories[directoryId].UploadFile(folderId, entity);

        await Clients.Group(directoryId).SendAsync("Upload",
            new TextFileReturnDTO { Id = entity.Id, Name = file.Name, Text = file.Text });
    }

    public async Task ConnectToRepository(string directoryId)
    {
        if (!_directories.ContainsKey(directoryId))
        {
            var dir = await _directoryService.Get(directoryId);
            
            _directories.Add(directoryId, new Directory(dir));
        }

        _connectionDirectory.Add(Context.ConnectionId, directoryId);

        await Groups.AddToGroupAsync(Context.ConnectionId, directoryId);
        await Clients.Caller.SendAsync("ConnectToRepository", _directories[directoryId]);
    }

    public async Task SaveRepository(string directoryId)
    {
        await _directories[directoryId].Save(_directoryService);
    }

    public async Task CreateFolder(string folderName, string directoryId, int parentFolderId)
    {
        if (_connectionDirectory[Context.ConnectionId] != directoryId)
            throw new Exception("Creating folder in not connected directory");

        

        var folder = _directories[directoryId].CreateFolder(new Folder(folderName), parentFolderId);
        var returnDto = new FolderReturnDTO
        {
            Id = folder.Id,
            Name = folder.Name,
            Files = folder.Files,
            Folders = folder.Folders
        };

        await Clients.Group(directoryId).SendAsync("CreateFolder", returnDto);
    }

    public async Task Change(string text, string directoryId, int fileId)
    {
        if (_connectionDirectory[Context.ConnectionId] != directoryId)
            throw new Exception("Changing file in not connected directory");

        _directories[directoryId].ChangeFile(text, fileId);
        //_codeSharing.Change(change, Context.ConnectionId);

        await Clients.Group(directoryId).SendAsync("Change", text, directoryId, fileId);
    }
}