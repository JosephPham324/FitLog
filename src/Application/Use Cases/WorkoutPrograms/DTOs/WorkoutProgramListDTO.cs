using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitLog.Domain.Entities;

namespace FitLog.Application.Use_Cases.WorkoutPrograms.DTOs;
public class WorkoutProgramListDTO
{
    public int ProgramId { get; set; }
    public string ProgramName { get; set; } = string.Empty;
    public string? ProgramThumbnail { get; set; }
    public int? NumberOfWeeks { get; set; }
    public int? DaysPerWeek { get; set; }
    public string? Goal { get; set; }
    public string? ExperienceLevel { get; set; }
    public string? GymType { get; set; }
    public string? MusclesPriority { get; set; }
    public string? AgeGroup { get; set; }
    public bool? PublicProgram { get; set; }
    public string? UserId { get; set; }
    public string? UserName { get; set; } // Map from User.UserName
    public string? CreatorFullName { get; set; }

    private class MappingProfile : AutoMapper.Profile
    {
        public MappingProfile()
        {
            CreateMap<Program, WorkoutProgramListDTO>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? src.User.UserName : "Unknown user"))
                .ForMember(dest => dest.CreatorFullName, opt => opt.MapFrom(src => src.User != null ? $"{src.User.FirstName} {src.User.LastName}" : "Unknown user"));
        }
    }
}
