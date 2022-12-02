using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMCS.Core.Domains.Meetings.Repositories;
using VMCS.Core.Domains.Meetings;
using VMCS.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity.Core;

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
            var meetingDb = new MeetingDbModel()
            {
                Id = meeting.Id,
                Name= meeting.Name,
                Channel = meeting.Channel,
                CreatedAt = meeting.CreatedAt,
                IsInChannel = meeting.IsInChannel,
            };

            _applicationContext.Meetings.Add(meetingDb);
        }

        public async Task Delete(string id, CancellationToken token)
        {
            var meeting = await _applicationContext.Meetings.FirstOrDefaultAsync(m => m.Id == id);

            if (meeting is null)
                throw new ObjectNotFoundException($"Meeting with id = {id} not found");

            _applicationContext.Remove(meeting);
        }

        public async Task<Meeting> GetMeetingByIdAsync(string id, CancellationToken token)
        {
            var meeting = await _applicationContext.Meetings.FirstOrDefaultAsync(m => m.Id == id);

            if (meeting is null)
                throw new ObjectNotFoundException($"Meeting with id = {id} not found");

            return new Meeting()
            {
                Id = meeting.Id,
                Name = meeting.Name,
                Channel = meeting.Channel,
                CreatedAt = meeting.CreatedAt,
                IsInChannel = meeting.IsInChannel,
            };
        }
    }
}
