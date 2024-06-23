using FitLog.Application.Users.Queries.GetUsers;
using FitLog.Domain.Entities;

namespace FitLog.Application.MuscleGroups.Queries.GetMuscleGroupsListWithPagination;

public class MuscleGroupDTO
{
    public int Id { get; set; }
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
