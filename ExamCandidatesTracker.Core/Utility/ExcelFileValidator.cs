using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamCandidatesTracker.Core.Utility
{
    public class ExcelFileValidator : AbstractValidator<IFormFile>
    {
        public ExcelFileValidator()
        {
            RuleFor(x => x.FileName).ExcelFileExtension();
        }
    }
}
