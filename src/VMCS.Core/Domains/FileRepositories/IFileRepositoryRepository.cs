namespace VMCS.Core.Domains.FileRepositories;

public interface IFileRepositoryRepository
{
    Task Add(FileRepository fileRepository);
}