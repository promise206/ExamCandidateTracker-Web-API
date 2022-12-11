using AutoMapper;
using Exam.Core.DTOs;
using Exam.Domain.Entities;

namespace Exam.Core.Utility
{
    public class ExamProfile : Profile
    {
        public ExamProfile()
        {
            CreateMap<CandidateDto, Candidate>().ReverseMap()
                 .ForMember(dest => dest.Email, act => act.MapFrom(src => src.Email.ToLower()));
            CreateMap<Attendance, AttendanceDto>().ReverseMap().ForMember(dest => dest.CandidateId, act => act.MapFrom(src => src.CandidateId)); ;
            CreateMap<AttendanceDto, Candidate>().ReverseMap().ForMember(dest => dest.CandidateId, act => act.MapFrom(src => src.Id)); 
        }
    }
}
