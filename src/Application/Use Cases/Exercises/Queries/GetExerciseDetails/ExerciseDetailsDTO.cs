using FitLog.Application.Users.Queries.GetUsers;
using FitLog.Domain.Entities;

namespace FitLog.Application.Exercises.Queries.GetExerciseDetails;

public class ExerciseDetailsDTO
{
    public int ExerciseId { get; set; }
    public string? CreatedBy { get; set; }
    public List<string> MuscleGroupNames { get; set; } = new List<string>();
    public int? EquipmentId { get; set; }
    public string? ExerciseName { get; set; }
    public string? DemoUrl { get; set; }
    public string Type { get; set; } = null!;
    public string? CreatedByName { get; set; }
    public string? EquipmentName { get; set; }

    private class Mapping : AutoMapper.Profile
    {
        public Mapping()
        {
            CreateMap<Exercise, ExerciseDetailsDTO>()
                .ForMember(dest => dest.CreatedByName, opt => opt.MapFrom(src => src.CreatedByNavigation.UserName))
                .ForMember(dest => dest.MuscleGroupNames, opt => opt.MapFrom(src => src.ExerciseMuscleGroups.Select(mg => mg.MuscleGroup.MuscleGroupName)))
                .ForMember(dest => dest.EquipmentName, opt => opt.MapFrom(src => src.Equipment != null ? src.Equipment.EquipmentName : "Name not found"));
        }
    }
}
