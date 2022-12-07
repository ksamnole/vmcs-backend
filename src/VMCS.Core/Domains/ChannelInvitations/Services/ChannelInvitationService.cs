using VMCS.Core.Domains.ChannelInvitations.Repositories;
using VMCS.Core.Domains.Channels.Services;
using VMCS.Core.Domains.Users.Services;

namespace VMCS.Core.Domains.ChannelInvitations.Services
{
    internal class ChannelInvitationService : IChannelInvitationService
    {
        private readonly IChannelService _channelService;
        private readonly IUserService _userService;
        private readonly IChannelInvitationRepository _channelInvitationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ChannelInvitationService(IChannelService channelService,
            IUserService userService,
            IChannelInvitationRepository channelInvitationRepository,
            IUnitOfWork unitOfWork)
        {
            _channelService = channelService;
            _userService = userService;
            _channelInvitationRepository = channelInvitationRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Accept(string invitationId, string userId, CancellationToken cancellationToken)
        {
            var invitation = await _channelInvitationRepository.Get(invitationId, cancellationToken);
            var channel = await _channelService.GetById(invitation.ChannelId, cancellationToken);
            var user = await _userService.GetById(userId, cancellationToken);
            
            await _channelService.AddUser(user, channel, cancellationToken);
            await _channelInvitationRepository.Delete(invitationId, cancellationToken);
            await _unitOfWork.SaveChange();
        }

        public async Task Create(ChannelInvitation invitation, CancellationToken cancellationToken)
        {
            var channel = await _channelService.GetById(invitation.ChannelId, cancellationToken);
            var user = await _userService.GetById(invitation.SenderId, cancellationToken);

            if (invitation.RecipientId == invitation.SenderId)
                throw new ValidationException("Sender and recipient are the same");
            
            if (!channel.Users.Contains(user))
                throw new ValidationException("User are not in this channel!");

            await _channelInvitationRepository.Create(invitation, cancellationToken);
            await _unitOfWork.SaveChange();
        }

        public async Task Decline(string invitationId, string userId, CancellationToken cancellationToken)
        {
            var invitation = await _channelInvitationRepository.Get(invitationId, cancellationToken);
            if (invitation.RecipientId != userId)
            {
                throw new ValidationException("User are not recipient!");
            }
            await _channelInvitationRepository.Delete(invitationId, cancellationToken);
            await _unitOfWork.SaveChange();
        }

        public async Task Delete(string invitationId, string userId, CancellationToken cancellationToken)
        {
            var invitation = await _channelInvitationRepository.Get(invitationId, cancellationToken);
            if (invitation.SenderId != userId)
            {
                throw new ValidationException("User are not sender!");
            }
            await _channelInvitationRepository.Delete(invitationId, cancellationToken);
            await _unitOfWork.SaveChange();
        }
    }
}
