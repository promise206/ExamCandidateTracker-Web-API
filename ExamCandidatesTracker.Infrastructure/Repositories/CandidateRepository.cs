using Exam.Core.Interfaces;
using Exam.Domain.Entities;

namespace Exam.Infrastructure.Repository
{
    public class CandidateRepository : GenericRepository<Candidate>, ICandidateRepository
    {
        public CandidateRepository(ExamDbContext context) : base(context)
        {
        }
    }
}
