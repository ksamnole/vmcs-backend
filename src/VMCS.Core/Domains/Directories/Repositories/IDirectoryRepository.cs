namespace VMCS.Core.Domains.Directories.Repositories;

public interface IDirectoryRepository
{
    Task Create(DirectoryDataModel directory);
    Task Delete(string directoryId);
    Task<DirectoryDataModel> Get(string directoryId);
    Task Save(DirectoryDataModel directory);
}