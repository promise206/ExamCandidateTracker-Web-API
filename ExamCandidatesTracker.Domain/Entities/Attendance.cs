using System.ComponentModel.DataAnnotations.Schema;

namespace Exam.Domain.Entities
{
    public class Attendance : BaseEntity
    {
        public string ExamNumber { get; set;}

        [ForeignKey("CandidateId")]
        public string CandidateId { get; set; }
        public virtual Candidate Candidate { get; set; }
        
    }
}
