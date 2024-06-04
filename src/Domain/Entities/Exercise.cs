using System;
using System.Collections.Generic;

namespace FitLog.Domain.Entities;

public partial class Exercise
{
    public int ExerciseId { get; set; }

    public int CreatedBy { get; set; }

    public int? MuscleGroupId { get; set; }

    public int? EquipmentId { get; set; }

    public string? ExerciseName { get; set; }

    public string? DemoUrl { get; set; }

    public string Type { get; set; } = null!;

    public string? Description { get; set; }

    public bool? PublicVisibility { get; set; }

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual Equipment? Equipment { get; set; }

    public virtual ICollection<ExerciseLog> ExerciseLogs { get; set; } = new List<ExerciseLog>();

    public virtual MuscleGroup? MuscleGroup { get; set; }

    public virtual ICollection<WorkoutTemplateExercise> WorkoutTemplateExercises { get; set; } = new List<WorkoutTemplateExercise>();
}
