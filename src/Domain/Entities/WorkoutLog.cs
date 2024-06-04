using System;
using System.Collections.Generic;

namespace FitLog.Domain.Entities;

public partial class WorkoutLog
{
    public int WorkoutLogId { get; set; }

    public int? CreatedBy { get; set; }

    public string? Note { get; set; }

    public TimeOnly? Duration { get; set; }

    public DateTime? LastModified { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual ICollection<ExerciseLog> ExerciseLogs { get; set; } = new List<ExerciseLog>();
}
