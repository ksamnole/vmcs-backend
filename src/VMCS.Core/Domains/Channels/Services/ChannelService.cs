using VMCS.Core.Domains.Channels.Repositories;

namespace VMCS.Core.Domains.Channels.Services;

public class ChannelService : IChannelService
{
    private readonly IChannelRepository _channelRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public ChannelService(IChannelRepository channelRepository, IUnitOfWork unitOfWork)
    {
        _channelRepository = channelRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Channel> GetById(string id, CancellationToken cancellationToken)
    {
        return await _channelRepository.GetById(id, cancellationToken);
    }

    public async Task Create(Channel channel, CancellationToken cancellationToken)
    {
        await _channelRepository.Create(channel, cancellationToken);
        await _unitOfWork.SaveChange();
    }

    public async Task Delete(string id, CancellationToken cancellationToken)
    {
        await _channelRepository.Delete(id, cancellationToken);
        await _unitOfWork.SaveChange();
    }
}