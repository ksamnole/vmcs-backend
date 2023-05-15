namespace VMCS.Core.Domains.Directories.Services;

public interface IDirectoryService
{
    Task<string> Create(Directory directory);
    Task Delete(string directoryId);
    Task<Directory> Get(string directoryId);
    Task Save(Directory directory);
    Task<string> Execute(string directoryId);
}