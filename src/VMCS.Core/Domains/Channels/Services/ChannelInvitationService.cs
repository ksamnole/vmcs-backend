using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using VMCS.Core.Domains.ChannelInvitations.Repositories;
using VMCS.Core.Domains.Channels.Repositories;
using VMCS.Core.Domains.Users;
using VMCS.Core.Domains.Users.Services;

namespace VMCS.Core.Domains.Channels.Services
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

        public async Task Accept(string userId, string channelId, CancellationToken cancellationToken)
        {
            var channel = await _channelService.GetById(channelId, cancellationToken);
            var user = await _userService.GetById(userId, cancellationToken);

            await _channelService.AddUser(user, channel, cancellationToken);
        }

        public async Task Create(ChannelInvitation invitation, CancellationToken cancellationToken)
        {
            var channel = await _channelService.GetById(invitation.ChannelId, cancellationToken);
            var user = await _userService.GetById(invitation.SenderId, cancellationToken);

            if (!channel.Users.Contains(user))
                throw new ValidationException("User are not in this channel!");

            await _channelInvitationRepository.Create(invitation, cancellationToken);
        }

        public async Task Decline(string userId, string channelId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(string invitaionId, CancellationToken cancellationToken)
        {
            await _channelInvitationRepository.Delete(invitaionId, cancellationToken);
        }
    }
}
