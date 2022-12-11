using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamCandidatesTracker.Core.Utility
{
    public static class AuthValidatorSettings
    {
        public static IRuleBuilder<T, string> ExamNumber<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            var options = ruleBuilder
                .Matches(@"^[0-9]{8}[A-Z]{2}$")
                .WithMessage("Incorrect exam number Format, must start with 8 digits and ends with 2 capital alphabets")
                .Length(10).WithMessage("Exam number must contain 8 character");
            return options;
        }

        public static IRuleBuilder<T, string> CenterCode<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            var options = ruleBuilder
                .Matches(@"^[A-Z]{2}[0-9]{4}$")
                .WithMessage("Incorrect centre code format, must start with 2 capital aphabets and ends with 4 digits")
                .Length(6).WithMessage("Exam number must contain 6 character");
            return options;
        }

        public static IRuleBuilder<T, string> ExcelFileExtension<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            var options = ruleBuilder
                .Must(a => a.EndsWith(".xlsx"))
                .WithMessage("Invalid file, upload excel file with .xlsx extension");
            return options;
        }
    }
}
