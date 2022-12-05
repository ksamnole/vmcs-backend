using FluentValidation;
using VMCS.Core.Domains.Channels.Repositories;
using VMCS.Core.Domains.Users.Services;

namespace VMCS.Core.Domains.Channels.Services;

public class ChannelService : IChannelService
{
    private readonly IChannelRepository _channelRepository;
    private readonly IUserService _userService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<Channel> _channelValidator;
    
    public ChannelService(IChannelRepository channelRepository, IUnitOfWork unitOfWork, IValidator<Channel> channelValidator, IUserService userService)
    {
        _channelRepository = channelRepository;
        _unitOfWork = unitOfWork;
        _channelValidator = channelValidator;
        _userService = userService;
    }
    
    public async Task<Channel> GetById(string id, CancellationToken cancellationToken)
    {
        return await _channelRepository.GetById(id, cancellationToken);
    }

    public async Task Create(Channel channel, CancellationToken cancellationToken)
    {
        var user = await _userService.GetById(channel.CreatorId, cancellationToken);
        channel.Users.Add(user);

        await _channelValidator.ValidateAndThrowAsync(channel, cancellationToken);
        
        await _channelRepository.Create(channel, cancellationToken);
        await _unitOfWork.SaveChange();
    }

    public async Task Delete(string id, CancellationToken cancellationToken)
    {
        await _channelRepository.Delete(id, cancellationToken);
        await _unitOfWork.SaveChange();
    }
}