using AutoMapper;
using Exam.Core.DTOs;
using Exam.Core.Interfaces;
using Exam.Domain.Entities;
using System.Net;

namespace Exam.Core.Services
{
    public class AttendanceService : IAttendanceService
    {
        public readonly IUnitOfWork _unitOfWork;
        public readonly IMapper _mapper;
        public AttendanceService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        /// <summary>
        /// Mark Candidate attendance
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<ResponseDto<bool>> MarkAttendance(AttendanceDto data)
        {
            try
            {
                var attendanceObject = _mapper.Map<Attendance>(data);
                await _unitOfWork.AttendanceRepository.Insert(attendanceObject);

                return ResponseDto<bool>.Success("Attendance Marked Successfully",true, (int)HttpStatusCode.OK);
            }
            catch
            {
                return ResponseDto<bool>.Fail("Unable to Mark Attendance",(int)HttpStatusCode.BadRequest);
            }
        }
        /// <summary>
        /// Check if Candidate Attendance Aready Taken
        /// </summary>
        /// <param name="examNumber"></param>
        /// <returns></returns>
        public async Task<bool> isCandidateAlreadyCheckedIn(string examNumber)
        {
            try
            {
                //Check If the Attendance is already taken
                var candidate = await _unitOfWork.AttendanceRepository.Get(x => x.ExamNumber == examNumber);
                if(candidate != null)
                {
                    return true;
                }

                return false;
            }
            catch(Exception) {
                throw;
            }
        }
    }
}
