using System.Data.Entity.Core;
using Microsoft.EntityFrameworkCore;
using VMCS.Core.Domains.Meetings;
using VMCS.Core.Domains.Meetings.Repositories;
using VMCS.Data.Contexts;

namespace VMCS.Data.Meetings.Repositories;

public class MeetingRepository : IMeetingRepository
{
    private readonly ApplicationContext _applicationContext;

    public MeetingRepository(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }

    public async Task Create(Meeting meeting, CancellationToken token)
    {
        await _applicationContext.Meetings.AddAsync(meeting, token);
    }

    public async Task Delete(string id, CancellationToken token)
    {
        var entity = await _applicationContext.Meetings.FirstOrDefaultAsync(m => m.Id == id, token);

        if (entity is null)
            throw new ObjectNotFoundException($"Meeting with id = {id} not found");

        _applicationContext.Meetings.Remove(entity);
    }

    public async Task<Meeting> GetMeetingByIdAsync(string id, CancellationToken token)
    {
        var entity = await _applicationContext.Meetings.FirstOrDefaultAsync(m => m.Id == id, token);

        if (entity is null)
            throw new ObjectNotFoundException($"Meeting with id = {id} not found");

        await _applicationContext.Chats.LoadAsync(token);
        await _applicationContext.Entry(entity).Collection(c => c.Users).LoadAsync(token);
        await _applicationContext.Messages.LoadAsync(token);
        await _applicationContext.Users.LoadAsync(token);

        entity.Chat.Messages = entity.Chat.Messages.OrderBy(x => x.ModifiedAt).ToList();

        return entity;
    }

    public async Task SetRepositoryToMeeting(string repositoryId, string meetingId, CancellationToken token)
    {
        var entity = await _applicationContext.Meetings.FirstOrDefaultAsync(x => x.Id == meetingId, token);

        if (entity == null)
            throw new ArgumentException($"Meeting with id : {meetingId} doesn't exists");

        if (entity.DirectoryId is not null)
            throw new InvalidOperationException("Meeting already has repository");

        entity.DirectoryId = repositoryId;
        await _applicationContext.SaveChangesAsync(token);
    }

    public bool ContainsById(string meetingId)
    {
        return _applicationContext
            .Meetings
            .Any(meeting => meeting.Id == meetingId);
    }
}