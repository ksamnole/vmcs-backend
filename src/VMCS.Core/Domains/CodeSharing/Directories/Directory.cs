using System.IO.Compression;
using System.Text.Json.Serialization;
using VMCS.Core.Domains.CodeSharing;
using VMCS.Core.Domains.CodeSharing.Models;
using VMCS.Core.Domains.Directories.Services;
using Newtonsoft.Json;

namespace VMCS.Core.Domains.CodeSharing.Directories;

public class Directory : IDirectory
{
    private readonly Dictionary<int, TextFile> _directoryFiles = new();
    private readonly Dictionary<int, Folder> _directoryFolders = new();
    private readonly UniqueIdentifierCreator _uniqueIdentifierCreator = new();
    public string Id { get; } = Guid.NewGuid().ToString();
    public string MeetingId { get; }
    public string Name { get; }
    public Folder RootFolder { get; }

    public Directory(Domains.Directories.Directory directory)
    {
        Id = directory.Id;
        Name = directory.Name;
        MeetingId = directory.MeetingId;
        RootFolder = string.IsNullOrEmpty(directory.DirectoryInJson) ? 
            new Folder() { 
                Name = directory.Name,
                Id = _uniqueIdentifierCreator.GetUniqueIdentifier()
            } :
            JsonConvert.DeserializeObject<Folder>(directory.DirectoryInJson);

        TraverseFolder(RootFolder);
    }

    private void TraverseFolder(Folder folder)
    {
        _directoryFolders.Add(folder.Id, folder);
        foreach (var file in folder.Files)
        {
            _directoryFiles.Add(file.Id, file);
        }
        foreach(var f in folder.Folders)
        {
            TraverseFolder(f);
        }
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

    public Folder CreateFolder(Folder folder, int parentFolderId)
    {
        folder.Id = _uniqueIdentifierCreator.GetUniqueIdentifier();

        if (!_directoryFolders.ContainsKey(parentFolderId))
            throw new ArgumentException($"Folder with id {parentFolderId} doesn`t exist");

        _directoryFolders.Add(folder.Id, folder);
        _directoryFolders[parentFolderId].Folders.Add(folder);

        return folder;
    }

    public void DeleteFolder(int folderId)
    {
        if (!_directoryFolders.ContainsKey(folderId) || _directoryFolders[folderId].IsDeleted)
            throw new ArgumentException($"Folder with id {folderId} doesn`t exist");

        _directoryFolders[folderId].IsDeleted = true;
    }

    public void DeleteFile(int fileId)
    {
        if (!_directoryFiles.ContainsKey(fileId) || _directoryFiles[fileId].IsDeleted)
            throw new ArgumentException($"File with id {fileId} doesn't exist");


        _directoryFiles[fileId].IsDeleted = true;
    }


    public void UploadFile(int folderId, TextFile file)
    {
        file.Id = _uniqueIdentifierCreator.GetUniqueIdentifier();
        var files = _directoryFolders[folderId].Files;
        files.Add(file);

        _directoryFiles.Add(file.Id, file);
    }

    public async Task Save(IDirectoryService directoryService)
    {
        RootFolder.DeleteDeletedObjects();

        var path = System.IO.Directory.GetCurrentDirectory();
        var repoPath = Path.Combine(path, Name);

        var folder = new DirectoryInfo(path);
        WriteFolder(RootFolder, folder);

        var zipPath = Path.Combine(path, $"{Name}.zip");

        ZipFile.CreateFromDirectory(
            repoPath,
            zipPath);

        System.IO.Directory.Delete(repoPath, true);


        var directory = new Domains.Directories.Directory()
        {
            Id = Id,
            DirectoryInJson = JsonConvert.SerializeObject(RootFolder),
            DirectoryZip = File.ReadAllBytes(zipPath),
            Name = Name,
            MeetingId = MeetingId,
        };

        File.Delete(zipPath);

        await directoryService.Save(directory);
    }

    public void ChangeFile(string text, int fileId)
    {
        if (!_directoryFiles.ContainsKey(fileId))
            throw new ArgumentException($"No file with id {fileId}");

        _directoryFiles[fileId].Text = text;
    }

    private void WriteFolder(Folder folder, DirectoryInfo currentFolder)
    {
        currentFolder = currentFolder.CreateSubdirectory(folder.Name);
        currentFolder.Create();
        foreach (var file in folder.Files)
        {
            //var text = new string(file.TextInBytes.Select(b => (char)b).ToArray());
            var text = file.Text;
            File.WriteAllText(Path.Combine(currentFolder.FullName, file.Name), text);
        }

        foreach (var subFolder in folder.Folders) WriteFolder(subFolder, currentFolder);
    }
}