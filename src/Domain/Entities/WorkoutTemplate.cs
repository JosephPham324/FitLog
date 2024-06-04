using System;
using System.Collections.Generic;

namespace FitLog.Domain.Entities;

public partial class WorkoutTemplate
{
    public int WorkoutTemplateId { get; set; }

    public int CreatedBy { get; set; }

    public string? TemplateName { get; set; }

    public string? Duration { get; set; }

    public int LastModifiedBy { get; set; }

    public DateTime? LastModified { get; set; }

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual User LastModifiedByNavigation { get; set; } = null!;

    public virtual ICollection<ProgramWorkout> ProgramWorkouts { get; set; } = new List<ProgramWorkout>();

    public virtual ICollection<WorkoutTemplateExercise> WorkoutTemplateExercises { get; set; } = new List<WorkoutTemplateExercise>();
}
