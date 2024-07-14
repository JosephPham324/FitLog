using FitLog.Application.Users.Queries.GetUsers;
using FitLog.Domain.Entities;

namespace FitLog.Application.WorkoutLogs.Queries.GetWorkoutLogsWithPagination;

public class ExerciseLogDTO
{
    public int ExerciseLogId { get; set; }
    public int? WorkoutLogId { get; set; }
    public int? ExerciseId { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime LastModified { get; set; }
    public int? OrderInSession { get; set; }
    public int? OrderInSuperset { get; set; }
    public string? Note { get; set; }
    public int? NumberOfSets { get; set; }
    public string? WeightsUsed { get; set; }
    public string? NumberOfReps { get; set; }
    public string? FootageUrls { get; set; }
    public string? ExerciseName { get; set; }

    public List<double>? GetWeightsUsed()
    {
        return WeightsUsed?
            .Trim(['[', ']'])?
            .Split([',', ';'])?
                                            //.Where(weight => !string.IsNullOrEmpty(weight))
            .Select(Double.Parse)?
            .ToList() ?? new List<double>();
    }

    public List<int>? GetNumberOfReps()
    {
        return NumberOfReps?
               .Trim(['[', ']'])?
                .Split([',', ';'])?
                //.Where(rep => !string.IsNullOrEmpty(rep))
                .Select(int.Parse)?
                .ToList();
    }

    private class Mapping : AutoMapper.Profile
    {
        public Mapping()
        {
            CreateMap<ExerciseLog, ExerciseLogDTO>()
                     .ForMember(dest => dest.ExerciseName, opt => opt.MapFrom(src => src.Exercise != null ? src.Exercise.ExerciseName : "Unknown exercise")); // Map ExerciseName from Exercise

        }
    }
}
