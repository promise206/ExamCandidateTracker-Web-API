using Exam.Core.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace Exam.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private bool _disposedValue;
        private readonly ExamDbContext _context;
        private IDbContextTransaction _objTransaction;
        private ICandidateRepository _candidateRepository;
        private IAttendanceRepository _attendanceRepository;
        public UnitOfWork(ExamDbContext context)
        {
            _context = context;
        }

        public ICandidateRepository CandidateRepository => _candidateRepository ??= new CandidateRepository(_context);
        public IAttendanceRepository AttendanceRepository => _attendanceRepository ??= new AttendanceRepository(_context);

        public async Task CreateTransaction()
        {
            _objTransaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task Commit()
        {
            await _objTransaction.CommitAsync();
        }

        public async Task Rollback()
        {
            await _objTransaction?.RollbackAsync();
            await _objTransaction.DisposeAsync();
        }


        public async Task Save()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw new Exception();
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _context.Dispose();
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
