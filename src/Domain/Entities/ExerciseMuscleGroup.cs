using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitLog.Domain.Entities;
public partial class ExerciseMuscleGroup
{
    public int ExerciseId { get; set; }
    public int MuscleGroupId { get; set; }

    public virtual Exercise Exercise { get; set; } = null!;
    public virtual MuscleGroup MuscleGroup { get; set; } = null!;
}

