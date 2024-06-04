using System;
using System.Collections.Generic;

namespace FitLog.Domain.Entities;

public partial class ExerciseLog
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

    public virtual Exercise? Exercise { get; set; }

    public virtual WorkoutLog? WorkoutLog { get; set; }
}
