using System;
using System.Collections.Generic;

namespace FitLog.Domain.Entities;

public partial class MuscleGroup
{
    public int MuscleGroupId { get; set; }
    public string? MuscleGroupName { get; set; }
    public string? ImageUrl { get; set; }

    public virtual ICollection<ExerciseMuscleGroup> ExerciseMuscleGroups { get; set; } = new List<ExerciseMuscleGroup>();
}
