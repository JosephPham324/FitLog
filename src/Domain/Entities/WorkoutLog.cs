using System;
using System.Collections.Generic;

namespace FitLog.Domain.Entities;

public partial class WorkoutLog
{
    public int WorkoutLogId { get; set; }

    public string? CreatedBy { get; set; }

    public string? Note { get; set; }

    public TimeOnly? Duration { get; set; }

    public DateTime? LastModified { get; set; }

    public virtual AspNetUser? CreatedByNavigation { get; set; }

    public virtual ICollection<ExerciseLog> ExerciseLogs { get; set; } = new List<ExerciseLog>();
}
