using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitLog.Application.CoachProfiles.Queries.GetCoachProfileDetails;

namespace FitLog.Application.Use_Cases.CoachProfiles.Queries.GetCoachProfileDetails;
public class ProgramOverviewDto
{
    public int ProgramId { get; set; }
    public string CreatorName { get; set; } = string.Empty;
    public string ProgramName { get; set; } = string.Empty;
    public string? ProgramThumbnail { get; set; }
    public int? NumberOfWeeks { get; set; }
    public int? DaysPerWeek { get; set; }
    public string? ExperienceLevel { get; set; }
    public string? GymType { get; set; }
    public string? MusclesPriority { get; set; }

    private class Mapping : AutoMapper.Profile
    {
        public Mapping()
        {
            CreateMap<Domain.Entities.Program, ProgramOverviewDto>()
                            .ForMember(dest => dest.CreatorName, opt => opt.MapFrom(src => src.User !=null ? $"{src.User.UserName}" : "Unknown User"));
        }
    }
}

