using FluentValidation;
using Moq;
using Xunit;
using VMCS.Core.Domains.Channels;
using VMCS.Core.Domains.Channels.Repositories;
using VMCS.Core.Domains.Channels.Services;
using VMCS.Core.Domains.Channels.Validators;
using VMCS.Core.Domains.Users;
using VMCS.Core.Domains.Users.Services;

namespace VMCS.Core.Tests.Tests;

public class ChannelServiceTests : TestBaseChannel
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
    
    [Fact]
    public async Task CreateChannel_SuccessPath_CreateCalledOneTime()
    {
        await _channelService.Create(Channel, CancellationToken.None);
        
        _mockChannelRepository.Verify(verify => verify.Create(Channel, CancellationToken.None));
        _mockUnitOfWork.Verify(verify => verify.SaveChange());
    }
    
    [Fact]
    public async Task CreateChannel_WithEmptyName_ShouldThrowException()
    {
        var channel = new Channel()
        {
            ChatId = "fakeChatId",
            CreatorId = "fakeCreatorId"
        };

        var exception = await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => _channelService.Create(channel, CancellationToken.None));
        var error = exception.Errors.First();
        
        Assert.Equal("Name", error.PropertyName);
        Assert.Equal("Please specify a Name", error.ErrorMessage);
    }
    
    [Fact]
    public async Task DeleteChannel_SuccessPath_DeleteCalledOneTime()
    {
        await _channelService.Delete(FakeId, CancellationToken.None);

        _mockChannelRepository.Verify(verify => verify.Delete(FakeId, CancellationToken.None));
        _mockUnitOfWork.Verify(verify => verify.SaveChange());
    }
    
    [Fact]
    public async Task AddUserToChannel_SuccessPath_ChannelGetUser()
    {
        await _channelService.AddUser(User, Channel, CancellationToken.None);

        _mockChannelRepository.Verify(verify => verify.AddUser(User, Channel, CancellationToken.None));
        _mockUnitOfWork.Verify(verify => verify.SaveChange());
    }
}

public class TestBaseChannel
{
    protected readonly Channel Channel;
    protected readonly User User;
    protected readonly string FakeId;

    protected TestBaseChannel()
    {
        Channel = new Channel()
        {
            Name = "name",
            ChatId = "fakeChatId",
            CreatorId = "fakeCreatorId"
        };
        
        User = new User()
        {
            Id = "fakeId",
            Login = "login",
            Username = "username",
            Email = "email"
        };

        FakeId = "fakeId";
    }
}