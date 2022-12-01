﻿namespace VMCS.Core.Domains.Users.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetById(string id, CancellationToken cancellationToken);
        Task<IEnumerable<User>> GetAll(CancellationToken cancellationToken);
        Task Create(User user, CancellationToken cancellationToken);
        Task Update(User user, CancellationToken cancellationToken);
        Task Delete(string id, CancellationToken cancellationToken);

        bool ContainsByLogin(string login);
    }
}