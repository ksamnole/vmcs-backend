namespace VMCS.Core
{
    public interface IUnitOfWork
    {
        Task<int> SaveChange();
    }
}
