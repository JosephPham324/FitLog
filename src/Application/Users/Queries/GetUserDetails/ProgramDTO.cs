using FitLog.Application.Users.Queries_.GetUsers;
using FitLog.Domain.Entities;

namespace FitLog.Application.Users.Queries.GetUserDetails;

public class ProgramDTO
{
    public int ProgramId { get; set; }
    public string? ProgramName { get; set; }
    public int? NumberOfWeeks { get; set; }
    public int? DaysPerWeek { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime LastModified { get; set; }
    public string? Goal { get; set; }
    public string? ExperienceLevel { get; set; }
    public string? GymType { get; set; }
    public string? MusclesPriority { get; set; }
    public string? AgeGroup { get; set; }
    public bool? PublicProgram { get; set; }

    private class Mapping : AutoMapper.Profile
    {
        public Mapping()
        {
            CreateMap<Program, ProgramDTO>();
        }
    }
}
