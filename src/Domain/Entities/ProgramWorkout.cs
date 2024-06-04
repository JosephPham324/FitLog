using System;
using System.Collections.Generic;

namespace FitLog.Domain.Entities;

public partial class ProgramWorkout
{
    public int ProgramWorkoutId { get; set; }

    public int? ProgramId { get; set; }

    public int? WorkoutTemplateId { get; set; }

    public int? WeekNumber { get; set; }

    public int? OrderInWeek { get; set; }

    public virtual Program? Program { get; set; }

    public virtual WorkoutTemplate? WorkoutTemplate { get; set; }
}
