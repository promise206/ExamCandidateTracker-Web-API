namespace Exam.Core.DTOs
{
    public class AttendanceDto
    {
        public string CandidateId { get; set; }
        public string ExamNumber { get; set; }
        public DateTimeOffset CheckedInDateTime { get; set; }
    }
}
