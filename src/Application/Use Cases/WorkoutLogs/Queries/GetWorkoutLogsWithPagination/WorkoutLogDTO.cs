using FitLog.Application.Users.Queries.GetUsers;
using FitLog.Domain.Entities;

namespace FitLog.Application.WorkoutLogs.Queries.GetWorkoutLogsWithPagination;

public class WorkoutLogDTO
{
    public int Id { get; set; }
    public string? CreatedBy { get; set; }
    public string? Note { get; set; }
    public TimeOnly? Duration { get; set; }
    public DateTimeOffset Created { get; set; }
    public DateTimeOffset? LastModified { get; set; }
    public List<ExerciseLogDTO> ExerciseLogs { get; set; } = new List<ExerciseLogDTO>();

    private class Mapping : AutoMapper.Profile
    {
        public Mapping()
        {
            CreateMap<WorkoutLog, WorkoutLogDTO>();
        }
    }
}
