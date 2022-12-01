using VMCS.Core.Domains.Users.Repositories;

namespace VMCS.Core.Domains.Users.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> GetById(string id, CancellationToken cancellationToken)
        {
            return await _userRepository.GetById(id, cancellationToken);
        }

        public async Task<IEnumerable<User>> GetAll(CancellationToken cancellationToken)
        {
            return await _userRepository.GetAll(cancellationToken);
        }

        public async Task Create(User user, CancellationToken cancellationToken)
        {
            await _userRepository.Create(user, cancellationToken);
        }

        public async Task Delete(string id, CancellationToken cancellationToken)
        {
            await _userRepository.Delete(id, cancellationToken);
        }

        public async Task Update(User user, CancellationToken cancellationToken)
        { 
            await _userRepository.Update(user, cancellationToken);
        }
    }
}
