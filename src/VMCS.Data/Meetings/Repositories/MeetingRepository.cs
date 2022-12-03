using VMCS.Core.Domains.Meetings.Repositories;
using VMCS.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity.Core;
using VMCS.Core.Domains.Meetings;

namespace VMCS.Data.Meetings.Repositories
{
    public class MeetingRepository : IMeetingRepository
    {
        private ApplicationContext _applicationContext;
        
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
            var meeting = await _applicationContext.Meetings.FirstOrDefaultAsync(m => m.Id == id, token);

            if (meeting is null)
                throw new ObjectNotFoundException($"Meeting with id = {id} not found");

            _applicationContext.Remove(meeting);
        }

        public async Task<Meeting> GetMeetingByIdAsync(string id, CancellationToken token)
        {
            var meeting = await _applicationContext.Meetings.FirstOrDefaultAsync(m => m.Id == id, token);
            
            if (meeting is null)
                throw new ObjectNotFoundException($"Meeting with id = {id} not found");

            return meeting;
        }
    }
}
