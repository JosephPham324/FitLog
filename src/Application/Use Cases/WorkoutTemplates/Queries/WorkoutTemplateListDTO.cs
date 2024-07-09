using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitLog.Domain.Entities;

namespace FitLog.Application.Use_Cases.WorkoutTemplates.Queries;
public class WorkoutTemplateListDto
{
    public int Id { get; set; }
    public string? TemplateName { get; set; }
    public string? Duration { get; set; }
    public string CreatorName { get; set; } = string.Empty;

    private class Mapping : AutoMapper.Profile
    {
        public Mapping()
        {
            CreateMap<WorkoutTemplate, WorkoutTemplateListDto>()
                .ForMember(d => d.CreatorName, opt => opt.MapFrom(s => s.CreatedByNavigation.UserName));
        }
    }
}
