using FluentValidation;
using VMCS.Core.Domains.Meetings.Repositories;

namespace VMCS.Core.Domains.Meetings.Services
{
    internal class MeetingService : IMeetingService
    {
        private readonly IMeetingRepository _meetingRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<Meeting> _meetingValidator;

        public MeetingService(IMeetingRepository meetingRepository, IUnitOfWork unitOfWork, IValidator<Meeting> meetingValidator)
        {
            _meetingRepository = meetingRepository;
            _unitOfWork = unitOfWork;
            _meetingValidator = meetingValidator;
        }

        public async Task Create(Meeting meeting, CancellationToken token)
        {
            await _meetingValidator.ValidateAndThrowAsync(meeting, token);
            
            await _meetingRepository.Create(meeting, token);
            await _unitOfWork.SaveChange();
        }

        public async Task Delete(string id, CancellationToken token)
        {
            await _meetingRepository.Delete(id, token);
            await _unitOfWork.SaveChange();
        }

        public Task<Meeting> GetMeetingByIdAsync(string id, CancellationToken token)
        {
            return _meetingRepository.GetMeetingByIdAsync(id, token);
        }
    }
}
