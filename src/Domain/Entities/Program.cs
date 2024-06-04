using System;
using System.Collections.Generic;

namespace FitLog.Domain.Entities;

public partial class Program
{
    public int ProgramId { get; set; }

    public string? UserId { get; set; }

    public string ProgramName { get; set; } = null!;

    public int? NumberOfWeeks { get; set; }

    public int? DaysPerWeek { get; set; }

    public DateTime DateCreated { get; set; }

    public DateTime LastModified { get; set; }

    public string? Goal { get; set; }

    public string? ExperienceLevel { get; set; }

    public string? GymType { get; set; }

    public string? MusclesPriority { get; set; }

    public string? AgeGroup { get; set; }

    public bool? PublicProgram { get; set; }

    public virtual ICollection<ProgramEnrollment> ProgramEnrollments { get; set; } = new List<ProgramEnrollment>();

    public virtual ICollection<ProgramWorkout> ProgramWorkouts { get; set; } = new List<ProgramWorkout>();

    public virtual AspNetUser? User { get; set; }
}
