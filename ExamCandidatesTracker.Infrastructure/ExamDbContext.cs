using Exam.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Exam.Infrastructure
{
    public class ExamDbContext : DbContext
    {
        public ExamDbContext(DbContextOptions<ExamDbContext> options) : base(options)
        {
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var item in ChangeTracker.Entries())
            {
                if (item.Entity is BaseEntity entity)
                {
                    switch (item.State)
                    {
                        case EntityState.Added:
                            entity.Id = Guid.NewGuid().ToString();
                            entity.CheckedInDateTime = DateTimeOffset.UtcNow;
                            break;
                        default:
                            break;
                    }
                }

                if (item.Entity is Candidate cadidateRoleEntity)
                    if (item.State == EntityState.Added) cadidateRoleEntity.Id = Guid.NewGuid().ToString();
            }
            return await base.SaveChangesAsync(cancellationToken);
        }

        public DbSet<Attendance> Attendance { get; set; }
        public DbSet<Candidate> Candidates { get; set; }
    }
}
