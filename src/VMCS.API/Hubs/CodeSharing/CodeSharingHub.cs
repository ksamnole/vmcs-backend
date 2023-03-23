using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using VMCS.API.Controllers.Channel.Dto;
using VMCS.API.Hubs.CodeSharing.Dto;
using VMCS.Core.Domains.CodeSharing.Directories;
using VMCS.Core.Domains.CodeSharing.Models;
using VMCS.Core.Domains.Directories.Services;

namespace VMCS.API.Hubs.CodeSharing;

public class CodeSharingHub : Hub
{
    private static readonly Dictionary<string, IDirectory> _directories = new();
    private static readonly Dictionary<string, string> _connectionDirectory = new();
    private readonly IDirectoryService _directoryService;
    private readonly IValidator<TextFile> _fileValidator;

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

    public async Task CreateFile(TextFileDTO file, int folderId, string directoryId)
    {
        var entity = new TextFile (file.Name, file.Text);

        if (!_connectionDirectory[Context.ConnectionId].Contains(directoryId))
            throw new Exception("Uploading file to not connected directory");

        await _fileValidator.ValidateAndThrowAsync(entity);

        _directories[directoryId].CreateFile(folderId, entity);

        await Clients.Group(directoryId).SendAsync("CreateFile",
            new TextFileReturnDto { Id = entity.Id, Name = file.Name, Text = file.Text, ParentId = folderId});
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
        var returnDto = new FolderReturnDto
        {
            Id = folder.Id,
            Name = folder.Name,
            Files = folder.Files,
            Folders = folder.Folders,
            ParentId = parentFolderId
        };

        await Clients.Group(directoryId).SendAsync("CreateFolder", returnDto);
    }

    public async Task Change(string changeJSON)
    {
        var change = JsonConvert.DeserializeObject<ChangeDTO>(changeJSON); 

        if (_connectionDirectory[Context.ConnectionId] != change.DirectoryId)
            throw new Exception("Changing file in not connected directory");

        _directories[change.DirectoryId].ChangeFile(change.FileId, change.Change);

        await Clients.Group(change.DirectoryId).SendAsync("Change", change);

        //if (_connectionDirectory[Context.ConnectionId] != changeDto.DirectoryId)
        //    throw new Exception("Changing file in not connected directory");
        //_directories[changeDto.DirectoryId].ChangeFile(changeDto.FileId, change);

        //await Clients.Group(changeDto.DirectoryId).SendAsync("Change", changeDto, change);
    }
}