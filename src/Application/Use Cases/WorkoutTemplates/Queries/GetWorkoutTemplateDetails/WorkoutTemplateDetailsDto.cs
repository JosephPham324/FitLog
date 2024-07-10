using FitLog.Application.Use_Cases.WorkoutTemplates.Queries;
using FitLog.Application.Use_Cases.WorkoutTemplates.Queries.GetWorkoutTemplateDetails;
using FitLog.Domain.Entities;

namespace FitLog.Application.WorkoutTemplates.Queries.GetWorkoutTemplateDetails;

public class WorkoutTemplateDetailsDto
{
    public int Id { get; set; }
    public string? TemplateName { get; set; }
    public string? Duration { get; set; }
    public string CreatorName { get; set; } = string.Empty;
    public ICollection<WorkoutTemplateExerciseDTO> WorkoutTemplateExercises { get; set; } = new List<WorkoutTemplateExerciseDTO>();

    private class Mapping : AutoMapper.Profile
    {
        public Mapping()
        {
            CreateMap<WorkoutTemplate, WorkoutTemplateDetailsDto>()
                .ForMember(d => d.CreatorName, opt => opt.MapFrom(s => s.CreatedByNavigation.UserName));
        }
    }
}
