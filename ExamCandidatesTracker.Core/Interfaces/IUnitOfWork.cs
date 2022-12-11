namespace Exam.Core.Interfaces
{
    public interface IUnitOfWork
    {
        ICandidateRepository CandidateRepository { get; }
        IAttendanceRepository AttendanceRepository { get; }
        Task Commit();
        Task CreateTransaction();
        void Dispose();
        Task Rollback();
        Task Save();
    }
}
