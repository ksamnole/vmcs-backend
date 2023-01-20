using FluentValidation;
using Moq;
using VMCS.Core.Domains.Channels;
using VMCS.Core.Domains.Channels.Repositories;
using VMCS.Core.Domains.Channels.Services;
using VMCS.Core.Domains.Channels.Validators;
using VMCS.Core.Domains.Users.Services;
using Xunit;

namespace VMCS.Core.Tests.Tests;

public class ChannelServiceTests
{
    private readonly IChannelService _channelService;
    private readonly IValidator<Channel> _channelValidator;
    private readonly Mock<IChannelRepository> _mockChannelRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IUserService> _mockUserService;

    public ChannelServiceTests()
    {
        _mockChannelRepository = new Mock<IChannelRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockUserService = new Mock<IUserService>();
        _channelValidator = new ChannelValidator();

        _channelService = new ChannelService(
            _mockChannelRepository.Object,
            _mockUnitOfWork.Object,
            _channelValidator,
            _mockUserService.Object
        );
    }

    [Fact]
    public async Task GetChannelById_SuccessPath_ReturnChannelModel()
    {
        _mockChannelRepository
            .Setup(repository => repository.GetById(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(new Channel { Id = "fakeId" });

        var channel = await _channelService.GetById("fakeId", CancellationToken.None);

        Assert.Equal(typeof(Channel), channel.GetType());
    }
}