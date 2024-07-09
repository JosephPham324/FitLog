using System;
using System.Collections.Generic;

namespace FitLog.Domain.Entities;

public partial class WorkoutTemplate : BaseAuditableEntity
{
    //public int WorkoutTemplateId { get; set; } //ID
    public string? TemplateName { get; set; } //Name
    public string? Duration { get; set; } //Duration
    public bool IsPublic { get; set; } //Viewable by all users or not
    public virtual AspNetUser CreatedByNavigation { get; set; } = null!;

    public virtual AspNetUser LastModifiedByNavigation { get; set; } = null!;

    public virtual ICollection<ProgramWorkout> ProgramWorkouts { get; set; } = new List<ProgramWorkout>();

    public virtual ICollection<WorkoutTemplateExercise> WorkoutTemplateExercises { get; set; } = new List<WorkoutTemplateExercise>();
}
