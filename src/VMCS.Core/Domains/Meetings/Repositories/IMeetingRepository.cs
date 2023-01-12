using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMCS.Core.Domains.Meetings.Repositories
{
    public interface IMeetingRepository
    {
        Task<Meeting> GetMeetingByIdAsync(string id, CancellationToken token);
        Task Create(Meeting meeting, CancellationToken token);
        Task Delete(string id, CancellationToken token);
        Task SetRepositoryToMeeting(string repositoryId, string meetingId, CancellationToken token);
    }
}
