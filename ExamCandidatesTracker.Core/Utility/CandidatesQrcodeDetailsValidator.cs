using Exam.Core.DTOs;
using FluentValidation;

namespace ExamCandidatesTracker.Core.Utility
{
    public class CandidatesQrcodeDetailsValidator : AbstractValidator<CandidatesQrcodeDetailsDto>
    {
        public CandidatesQrcodeDetailsValidator()
        {
            RuleFor(CandidatesQrcodeDetailsDto => CandidatesQrcodeDetailsDto.ExamNumber).NotEmpty().WithMessage("Invalid Exam Number").ExamNumber();
            RuleFor(CandidatesQrcodeDetailsDto => CandidatesQrcodeDetailsDto.CenterCode).NotEmpty().WithMessage("Invalid Center Code").CenterCode();
        }
    }
}