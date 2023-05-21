using VMCS.Core.Domains.CodeExecution.HttpClients;
using VMCS.Core.Domains.Directories.Repositories;
using VMCS.Core.Domains.Meetings.Services;

namespace VMCS.Core.Domains.Directories.Services;

public class DirectoryService : IDirectoryService
{
    private readonly ICodeExecutor _codeExecutor;
    private readonly IDirectoryRepository _directoryRepository;
    private readonly IMeetingService _meetingService;
    private readonly IUnitOfWork _unitOfWork;

    public DirectoryService(IDirectoryRepository directoryRepository, IUnitOfWork unitOfWork,
        IMeetingService meetingService, ICodeExecutor codeExecutor)
    {
        _directoryRepository = directoryRepository;
        _unitOfWork = unitOfWork;
        _meetingService = meetingService;
        _codeExecutor = codeExecutor;
    }

    public async Task<string> Create(Directory directory)
    {
        await _directoryRepository.Create(directory);
        await _meetingService.SetDirectoryToMeeting(directory.Id, directory.MeetingId, CancellationToken.None);
        await _unitOfWork.SaveChange();

        return directory.Id;
    }

    public async Task Delete(string directoryId)
    {
        await _directoryRepository.Delete(directoryId);
        await _unitOfWork.SaveChange();
    }

    public async Task<string> Execute(string directoryId)
    {
        var directory = await Get(directoryId);

        return await _codeExecutor.ExecuteAsync(directory.DirectoryZip, directory.Language);
    }

    public async Task<Directory> Get(string directoryId)
    {
        return await _directoryRepository.Get(directoryId);
    }

    public async Task Save(Directory directory)
    {
        await _directoryRepository.Save(directory);
        await _unitOfWork.SaveChange();
    }
}