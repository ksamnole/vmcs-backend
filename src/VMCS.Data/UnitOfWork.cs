using VMCS.Core;
using VMCS.Data.Contexts;

namespace VMCS.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationContext _applicationContext;
    private readonly AuthenticationContext _authenticationContext;

    public UnitOfWork(ApplicationContext applicationContext, AuthenticationContext authenticationContext)
    {
        _applicationContext = applicationContext;
        _authenticationContext = authenticationContext;
    }

    public Task<int> SaveChange()
    {
        _authenticationContext.SaveChanges();
        return _applicationContext.SaveChangesAsync();
    }
}