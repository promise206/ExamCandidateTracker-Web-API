namespace Exam.Core.DTOs
{
    public class EmailDto
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
    }
}
