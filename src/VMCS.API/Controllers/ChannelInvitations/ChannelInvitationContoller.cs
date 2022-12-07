﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using VMCS.API.Controllers.ChannelInvitations.Dto;
using VMCS.Core.Domains.ChannelInvitations;
using VMCS.Core.Domains.ChannelInvitations.Services;
using VMCS.Core.Domains.Channels;

namespace VMCS.API.Controllers.ChannelInvitations
{

    [Route("channelInvitation")]
    [ApiController]
    [Authorize]
    public class ChannelInvitationContoller : ControllerBase
    {
        private readonly IChannelInvitationService _channelInvitationService;

        public ChannelInvitationContoller(IChannelInvitationService channelInvitationService)
        {
            _channelInvitationService = channelInvitationService;
        }

        [Route("accept/{id}")]
        [HttpGet]
        public async Task Accept(string invitationId, CancellationToken cancellationToken)
        {
            await _channelInvitationService.Accept(invitationId, User.FindFirstValue(ClaimTypes.NameIdentifier), cancellationToken);
        }

        [Route("delete/{id}")]
        [HttpGet]
        public async Task Delete(string invitationId, CancellationToken cancellationToken)
        {
            await _channelInvitationService.Delete(invitationId, User.FindFirstValue(ClaimTypes.NameIdentifier), cancellationToken);
        }

        [Route("decline/{id}")]
        [HttpGet]
        public async Task Decline(string invitationId, CancellationToken cancellationToken)
        {
            await _channelInvitationService.Delete(invitationId, User.FindFirstValue(ClaimTypes.NameIdentifier), cancellationToken);
        }


        [Route("create")]
        [HttpPost]
        public async Task Create(ChannelInvitationRequestDto request, CancellationToken cancellationToken)
        {
            var invitation = new ChannelInvitation()
            {
                RecipientId = request.RecipientId,
                ChannelId = request.ChannelId,
                SenderId = User.FindFirstValue(ClaimTypes.NameIdentifier),
            };

            await _channelInvitationService.Create(invitation, cancellationToken);
        }
    }
}
