using FitLog.Application.Users.Queries.GetUsers;
using FitLog.Domain.Entities;

namespace FitLog.Application.Exercises.Queries.GetExerciseDetails;

public class ExerciseDetailsDTO
{
    public int ExerciseId { get; set; }
    public string? CreatedById { get; set; }
    public int? MuscleGroupId { get; set; }
    public int? EquipmentId { get; set; }
    public string? ExerciseName { get; set; }
    public string? DemoUrl { get; set; }
    public string Type { get; set; } = null!;


    public string? CreatedByName { get; set; }
    public string? MuscleGroupName { get; set; }
    public string? EquipmentName { get; set; }

    private class Mapping : AutoMapper.Profile
    {
        public Mapping()
        {
        //     public virtual Equipment? Equipment { get; set; }

        //public virtual ICollection<ExerciseLog> ExerciseLogs { get; set; } = new List<ExerciseLog>();

        //public virtual MuscleGroup? MuscleGroup { get; set; }

        CreateMap<Exercise, ExerciseDetailsDTO>()
            .ForMember(dest => dest.CreatedByName, opt => opt.MapFrom(src => src.CreatedByNavigation.UserName))
            .ForMember(dest => dest.MuscleGroupName, opt => opt.MapFrom(src => src.MuscleGroup != null ? src.MuscleGroup.MuscleGroupName : "Name not found"))
            .ForMember(dest => dest.EquipmentName, opt=> opt.MapFrom(src=>src.Equipment != null ? src.Equipment.EquipmentName : "Name not found"));
        }
    }
}
