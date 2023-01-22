namespace VMCS.Core.Domains.Directories.Services;

public interface IDirectoryService
{
    Task<Directory> Create(Directory directory);
    Task Delete(string directoryId);
    Task<Directory> Get(string directoryId);
    Task Save(Directory directory);
}