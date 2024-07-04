using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitLog.Application.TodoItems.Queries.GetTodoItemsWithPagination;
using FitLog.Domain.Entities;

namespace FitLog.Application.TrainingSurveys.Commands;
public class TrainingSurveyDTO
{
    [Required]
    public string? UserId { get; set; }

    public string? Goal { get; set; }

    public int? DaysPerWeek { get; set; }

    public string? ExperienceLevel { get; set; }

    public string? GymType { get; set; }

    public string? MusclesPriority { get; set; }

    public int? Age { get; set; }

    public DateTime LastModified { get; set; }

    public TrainingSurveyDTO()
    {
        // Set default value for LastModified
        LastModified = DateTime.UtcNow;
    }

    private class Mapping : AutoMapper.Profile
    {
        public Mapping()
        {
            CreateMap<SurveyAnswer, TrainingSurveyDTO>();
        }
    }
}
