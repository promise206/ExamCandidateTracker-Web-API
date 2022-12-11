using Exam.Core.DTOs;
using Microsoft.AspNetCore.Http;

namespace Exam.Core.Interfaces
{
    public interface ICandidateService
    {
        Task<ResponseDto<List<CandidateDto>>> UploadExcelSheet(IFormFile candidatesSheet);
        Task<ResponseDto<List<CandidateDto>>> GetAllCandidates();
        Task<ResponseDto<bool>> DeleteAllCandidates();
    }
}