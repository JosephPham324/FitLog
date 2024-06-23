using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitLog.Application.MuscleGroups.Commands.UpdateMuscleGroup;
public class UpdateMuscleGroupDTO
{
    public bool Success { get; set; }
    public IEnumerable<string> Errors { get; set; } = new List<string>();
}
