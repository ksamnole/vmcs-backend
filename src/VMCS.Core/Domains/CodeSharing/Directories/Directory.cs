using System.IO.Compression;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using VMCS.Core.Domains.CodeSharing.Models;
using VMCS.Core.Domains.Directories.Services;
using VMCS.Core.Extensions;

namespace VMCS.Core.Domains.CodeSharing.Directories;

public class Directory : IDirectory
{
    private readonly Dictionary<int, TextFile> _directoryFiles = new();
    private readonly Dictionary<int, Folder> _directoryFolders = new();
    private readonly UniqueIdentifierCreator _uniqueIdentifierCreator = new();
    private string syncObj = "";

    public Directory(Domains.Directories.DirectoryDataModel directory)
    {
        Id = directory.Id;
        Name = directory.Name;
        MeetingId = directory.MeetingId;
        RootFolder = GetRootFolder(directory);

        TraverseFolder(RootFolder);
    }

    public Directory(string meetingId, string name)
    {
        MeetingId = meetingId;
        Name = name;
        RootFolder = new Folder
        {
            Id = _uniqueIdentifierCreator.GetUniqueIdentifier(),
            Name = name
        };

        _directoryFolders.Add(RootFolder.Id, RootFolder);
    }

    public string Id { get; } = Guid.NewGuid().ToString();
    public string MeetingId { get; }
    public string Name { get; }
    public Folder RootFolder { get; }

    public Folder CreateFolder(Folder folder, int parentFolderId)
    {
        folder.Id = _uniqueIdentifierCreator.GetUniqueIdentifier();

        if (!_directoryFolders.ContainsKey(parentFolderId))
            throw new ArgumentException($"Folder with id {parentFolderId} doesn't exist");

        _directoryFolders.Add(folder.Id, folder);
        _directoryFolders[parentFolderId].Folders.Add(folder);

        return folder;
    }

    public void DeleteFolder(int folderId)
    {
        if (!_directoryFolders.ContainsKey(folderId) || _directoryFolders[folderId].IsDeleted)
            throw new ArgumentException($"Folder with id {folderId} doesn't exist");

        _directoryFolders[folderId].IsDeleted = true;
        RootFolder.DeleteDeletedObjects();
    }

    public void DeleteFile(int fileId)
    {
        if (!_directoryFiles.ContainsKey(fileId) || _directoryFiles[fileId].IsDeleted)
            throw new ArgumentException($"File with id {fileId} doesn't exist");
        
        _directoryFiles[fileId].IsDeleted = true;
        RootFolder.DeleteDeletedObjects();
    }

    public void CreateFile(TextFile file, int folderId)
    {
        file.Id = _uniqueIdentifierCreator.GetUniqueIdentifier();
        var files = _directoryFolders[folderId].Files;
        files.Add(file);

        _directoryFiles.Add(file.Id, file);
    }

    public async Task Save(IDirectoryService directoryService)
    {
        RootFolder.DeleteDeletedObjects();

        var memoryStream = new MemoryStream();
        var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true);

        WriteFolder(RootFolder, "", zipArchive);

        zipArchive.Dispose();
        
        var directory = new Domains.Directories.DirectoryDataModel
        {
            Id = Id,
            DirectoryInJson = JsonConvert.SerializeObject(RootFolder),
            DirectoryZip = memoryStream.ToArray(),
            Name = Name,
            MeetingId = MeetingId
        };

        await directoryService.Save(directory);
    }

    private static void WriteFolder(Folder folder, string folderPath, ZipArchive zipArchive)
    {
        foreach (var file in folder.Files)
        {
            var text = file.Text;
            zipArchive.AddTextFile(Path.Combine(folderPath, file.Name), text);
        }

        foreach (var subFolder in folder.Folders)
        {
            var subFPath = Path.Combine(folderPath, subFolder.Name + "\\");
            zipArchive.CreateEntry(subFPath);
            WriteFolder(subFolder, subFPath, zipArchive);
        }
    }

    public void ChangeFile(int fileId, string text)
    {
        if (!_directoryFiles.ContainsKey(fileId))
            throw new ArgumentException($"No file with id {fileId}");

        var file = _directoryFiles[fileId];

        file.Text = text;
    }

    private Folder GetRootFolder(Domains.Directories.DirectoryDataModel directory)
    {
        var folder = new Folder { Name = directory.Name, Id = _uniqueIdentifierCreator.GetUniqueIdentifier() };
        
        if (string.IsNullOrEmpty(directory.DirectoryInJson))
            return folder;

        var deserializeFolder = JsonConvert.DeserializeObject<Folder>(directory.DirectoryInJson);

        return deserializeFolder ?? folder;
    }

    private void TraverseFolder(Folder folder)
    {
        _directoryFolders.Add(folder.Id, folder);
        foreach (var file in folder.Files) _directoryFiles.Add(file.Id, file);
        foreach (var f in folder.Folders) TraverseFolder(f);
    }
}