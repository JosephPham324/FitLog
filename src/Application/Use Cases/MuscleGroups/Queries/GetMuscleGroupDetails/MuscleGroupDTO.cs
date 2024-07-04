using FitLog.Domain.Entities;

namespace FitLog.Application.MuscleGroups.Queries.GetMuscleGroupDetails;

public class MuscleGroupDTO
{
    public int MuscleGroupId { get; set; }
    public string? MuscleGroupName { get; set; }

    public string? ImageUrl { get; set; }

    private class Mapping : AutoMapper.Profile
    {
        public Mapping()
        {
            CreateMap<MuscleGroup, MuscleGroupDTO>();
        }
    }
}
