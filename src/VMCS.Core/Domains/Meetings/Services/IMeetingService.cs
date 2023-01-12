using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMCS.Core.Domains.Meetings.Services
{
    public interface IMeetingService
    {
        Task<Meeting> GetMeetingByIdAsync(string id, CancellationToken token);
        Task<Meeting> Create(Meeting meeting, CancellationToken token);
        Task Delete(string id, CancellationToken token);
        Task SetRepositoryToMeeting(string repositoryId, string meetingId, CancellationToken token);
    }
}
