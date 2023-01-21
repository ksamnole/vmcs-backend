using System.Data.Entity.Core;
using Microsoft.EntityFrameworkCore;
using VMCS.Core.Domains.Directories.Repositories;
using VMCS.Data.Contexts;
using Directory = VMCS.Core.Domains.Directories.Directory;

namespace VMCS.Data.Directories;

public class DirectoryRepository : IDirectoryRepository
{
    private readonly ApplicationContext _applicationContext;

    public DirectoryRepository(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }

    public async Task Create(Directory directory)
    {
        await _applicationContext.Directories.AddAsync(directory);
    }

    public async Task Delete(string directoryId)
    {
        var entity = await _applicationContext.Directories.FirstOrDefaultAsync(x => x.Id == directoryId);

        if (entity is null) throw new ObjectNotFoundException($"Directory with id = {directoryId} not found.");

        _applicationContext.Directories.Remove(entity);
    }

    public async Task<Directory> Get(string directoryId)
    {
        var entity = await _applicationContext.Directories.FirstOrDefaultAsync(x => x.Id == directoryId);

        if (entity is null) throw new ObjectNotFoundException($"Directory with id = {directoryId} not found.");

        return entity;
    }

    public async Task Save(Directory directory)
    {
        var entity = await _applicationContext.Directories.FirstOrDefaultAsync(x => x.Id == directory.Id);

        if (entity is null) throw new ObjectNotFoundException($"Directory with id = {directory.Id} not found.");

        entity.DirectoryZip = directory.DirectoryZip;
        entity.DirectoryInJson = directory.DirectoryInJson;
    }
}