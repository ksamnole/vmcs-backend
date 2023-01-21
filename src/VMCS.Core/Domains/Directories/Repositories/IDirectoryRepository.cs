namespace VMCS.Core.Domains.Directories.Repositories;

public interface IDirectoryRepository
{
    Task Create(Directory directory);
    Task Delete(string directoryId);
    Task<Directory> Get(string directoryId);
    Task Save(Directory directory);
}