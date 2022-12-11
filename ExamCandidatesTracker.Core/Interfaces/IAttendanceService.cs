using Exam.Core.DTOs;

namespace Exam.Core.Interfaces
{
    public interface IAttendanceService
    {
        Task<ResponseDto<bool>> MarkAttendance(AttendanceDto data);
        Task<bool> isCandidateAlreadyCheckedIn(string examNumber);
    }
}