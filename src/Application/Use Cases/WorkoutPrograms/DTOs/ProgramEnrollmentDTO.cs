using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitLog.Domain.Entities;

namespace FitLog.Application.Use_Cases.WorkoutPrograms.DTOs;
public class ProgramEnrollmentDTO
{
    public int EnrollmentId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public int ProgramId { get; set; }
    public DateTime EnrolledDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public int? CurrentWeekNo { get; set; }
    public int? CurrentWorkoutOrder { get; set; } 
    public string ProgramName { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;

    private class AutoMapperProfile : AutoMapper.Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ProgramEnrollment, ProgramEnrollmentDTO>()
                .ForMember(dest => dest.ProgramName, opt => opt.MapFrom(src => src.Program!= null? src.Program.ProgramName : "Unknown program"))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null? src.User.UserName : "Unknown user"));
        }
    }
}
