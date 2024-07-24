using System;
using System.Collections.Generic;

namespace FitLog.Domain.Entities;

public partial class ProgramEnrollment
{
    public int EnrollmentId { get; set; }

    public string? UserId { get; set; }

    public int? ProgramId { get; set; }

    public DateTime EnrolledDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string Status { get; set; } = null!;

    public int? CurrentWeekNo { get; set; }

    public int? CurrentWorkoutOrder { get; set; }

    public virtual Program? Program { get; set; }

    public virtual AspNetUser? User { get; set; }
    public Dictionary<int, WorkoutProgress> WorkoutsProgress { get; set; } = new Dictionary<int, WorkoutProgress>();

    public record WorkoutProgress
    {
        public DateTime DateCompleted { get; set; }
        public string Status { get; set; } = null!;
        public string Notes { get; set; } = null!;
    }
}
