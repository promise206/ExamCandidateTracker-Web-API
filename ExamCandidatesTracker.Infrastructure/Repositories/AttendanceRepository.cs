using Exam.Core.Interfaces;
using Exam.Domain.Entities;

namespace Exam.Infrastructure.Repository
{
    public class AttendanceRepository : GenericRepository<Attendance>, IAttendanceRepository
    {
        public AttendanceRepository(ExamDbContext context) : base(context)
        {
        }
        
    }
}
