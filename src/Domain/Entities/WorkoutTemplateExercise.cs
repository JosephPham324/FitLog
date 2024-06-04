using System;
using System.Collections.Generic;

namespace FitLog.Domain.Entities;

public partial class WorkoutTemplateExercise
{
    public int ExerciseTemlateId { get; set; }

    public int? WorkoutTemplateId { get; set; }

    public int? ExerciseId { get; set; }

    public int? OrderInSession { get; set; }

    public int? OrderInSuperset { get; set; }

    public string? Note { get; set; }

    public int? SetsRecommendation { get; set; }

    public int? IntensityPercentage { get; set; }

    public int? RpeRecommendation { get; set; }

    public string? WeightsUsed { get; set; }

    public string? NumbersOfReps { get; set; }

    public virtual Exercise? Exercise { get; set; }

    public virtual WorkoutTemplate? WorkoutTemplate { get; set; }
}
