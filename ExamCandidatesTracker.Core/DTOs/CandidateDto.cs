namespace Exam.Core.DTOs
{
    public class CandidateDto
    {
        public string FullName { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string ExamNumber { get; set; }
        public DateTimeOffset ExameDateTime { get; set; }
        public string CentreCode { get; set; }
        public AttendanceDto Attendance { get; set; }
    }
}
