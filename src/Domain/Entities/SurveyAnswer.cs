using System;
using System.Collections.Generic;

namespace FitLog.Domain.Entities;

public partial class SurveyAnswer
{
    public int SurveyAnswerId { get; set; }

    public int? UserId { get; set; }

    public string? Goal { get; set; }

    public int? DaysPerWeek { get; set; }

    public string? ExperienceLevel { get; set; }

    public string? GymType { get; set; }

    public string? MusclesPriority { get; set; }

    public int? Age { get; set; }

    public DateTime LastModified { get; set; }

    public virtual User? User { get; set; }
}
