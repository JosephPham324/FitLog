using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitLog.Application.Exercises.Queries.GetExercises;
using FitLog.Application.WorkoutTemplates.Queries.GetWorkoutTemplateDetails;
using FitLog.Domain.Entities;

namespace FitLog.Application.Use_Cases.WorkoutTemplates.Queries.GetWorkoutTemplateDetails;
public class WorkoutTemplateExerciseDTO
{
    public int ExerciseTemlateId { get; set; }
    public int? OrderInSession { get; set; }
    public int? OrderInSuperset { get; set; }
    public string? Note { get; set; }

    public int? SetsRecommendation { get; set; }

    public int? IntensityPercentage { get; set; }

    public int? RpeRecommendation { get; set; }

    public string? WeightsUsed { get; set; }

    public string? NumbersOfReps { get; set; }

    public virtual ExerciseDTO? Exercise { get; set; }

    private class Mapping : AutoMapper.Profile
    {
        public Mapping()
        {
            CreateMap<WorkoutTemplateExercise, WorkoutTemplateExerciseDTO>();
        }
    }
}
