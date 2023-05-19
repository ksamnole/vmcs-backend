namespace VMCS.Core.Domains.Directories.Services;

public interface IDirectoryService
{
    Task<string> Create(DirectoryDataModel directory);
    Task Delete(string directoryId);
    Task<DirectoryDataModel> Get(string directoryId);
    Task Save(DirectoryDataModel directory);
    Task<string> Execute(string directoryId);
}