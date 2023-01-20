using VMCS.Core;
using VMCS.Data.Contexts;

namespace VMCS.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationContext _applicationContext;

    public UnitOfWork(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }

    public Task<int> SaveChange()
    {
        return _applicationContext.SaveChangesAsync();
    }
}