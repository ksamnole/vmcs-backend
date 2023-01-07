using FluentValidation;
using VMCS.Core.Domains.ChannelInvitations;
using VMCS.Core.Domains.Channels;
using VMCS.Core.Domains.Users.Repositories;

namespace VMCS.Core.Domains.Users.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<User> _userValidator;

        public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork, IValidator<User> userValidator)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _userValidator = userValidator;
        }

        public async Task<User> GetById(string id, CancellationToken cancellationToken)
        {
            return await _userRepository.GetById(id, cancellationToken);
        }

        public async Task<IEnumerable<User>> GetAll(CancellationToken cancellationToken)
        {
            return await _userRepository.GetAll(cancellationToken);
        }

        public async Task<IEnumerable<Channel>> GetAllUserChannels(string userId, CancellationToken cancellationToken)
        {
            return await _userRepository.GetAllUserChannels(userId, cancellationToken);
        }

        public async Task<IEnumerable<ChannelInvitation>> GetAllUserChannelInvitations(string userId, CancellationToken cancellationToken)
        {
            return await _userRepository.GetAllUserChannelInvitations(userId, cancellationToken);
        }

        public async Task Create(User user, CancellationToken cancellationToken)
        {
            await _userValidator.ValidateAndThrowAsync(user, cancellationToken);
            
            await _userRepository.Create(user, cancellationToken);
            await _unitOfWork.SaveChange();
        }

        public async Task Delete(string id, CancellationToken cancellationToken)
        {
            await _userRepository.Delete(id, cancellationToken);
            await _unitOfWork.SaveChange();
        }

        public async Task Update(User user, CancellationToken cancellationToken)
        {
            var currentUser = await _userRepository.GetById(user.Id, cancellationToken);
            var changeUser = new User
            {
                Id = user.Id,
                Login = currentUser.Login,
                Email = string.IsNullOrEmpty(user.Email) ? currentUser.Email : user.Email,
                Username = string.IsNullOrEmpty(user.Username) ? currentUser.Username : user.Username
            };
            
            await _userValidator.ValidateAndThrowAsync(changeUser, cancellationToken);
            
            await _userRepository.Update(user, cancellationToken);
            await _unitOfWork.SaveChange();
        }
    }
}
