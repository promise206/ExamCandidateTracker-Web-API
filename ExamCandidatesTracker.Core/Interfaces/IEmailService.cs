using Exam.Core.DTOs;

namespace Exam.Core.Interfaces
{
    public interface IEmailService
    {
        Task<ResponseDto<bool>> SendEmail(EmailDto model);
    }
}