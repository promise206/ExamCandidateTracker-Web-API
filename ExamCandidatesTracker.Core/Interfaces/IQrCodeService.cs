using Exam.Core.DTOs;
using Exam.Domain.Entities;

namespace Exam.Core.Interfaces
{
    public interface IQrCodeService
    {
        Task<ResponseDto<CandidateDto>> VerifyCandidateQrCodeDetails(CandidatesQrcodeDetailsDto candidateQrCodeDetail);
        Task<ResponseDto<string>> GenerateQrCodeForCandidates(Candidate candidatesQrCodeDetails);
    }
}