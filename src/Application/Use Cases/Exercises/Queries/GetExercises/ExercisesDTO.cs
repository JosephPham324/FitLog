using FitLog.Domain.Entities;

namespace FitLog.Application.Exercises.Queries.GetExercises;

public class ExerciseDTO
{
    public int ExerciseId { get; set; }
    public string? ExerciseName { get; set; }
    public string Type { get; set; } = null!;

    private class Mapping : AutoMapper.Profile
    {
        public Mapping()
        {
            CreateMap<Exercise, ExerciseDTO>();
        }
    }
}
