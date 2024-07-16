using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitLog.Application.WorkoutTemplates.Queries.GetWorkoutTemplateDetails;
using FitLog.Domain.Entities;

namespace FitLog.Application.Use_Cases.WorkoutPrograms.DTOs;
public class ProgramWorkoutDto
{
    public int ProgramWorkoutId { get; set; }
    public int? WeekNumber { get; set; }
    public int? OrderInWeek { get; set; }
    public WorkoutTemplateDetailsDto? WorkoutTemplate { get; set; }
    private class MappingProfile : AutoMapper.Profile
    {
        public MappingProfile()
        {
            CreateMap<ProgramWorkout, ProgramWorkoutDto>();
        }
    }
}
