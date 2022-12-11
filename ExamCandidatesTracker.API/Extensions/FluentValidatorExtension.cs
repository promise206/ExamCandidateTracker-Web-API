using Exam.Core.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using System.Reflection;

namespace ExamCandidatesTracker.API.Extensions
{
    public static class FluentValidatorExtension
    {
        public static void AddFluentValidatorExtension(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
            services.AddValidatorsFromAssemblyContaining<CandidateService>();
        }
    }
}
