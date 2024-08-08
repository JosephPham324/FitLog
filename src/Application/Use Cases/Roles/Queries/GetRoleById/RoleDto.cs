using FitLog.Domain.Entities;

namespace FitLog.Application.Roles.Queries.GetRoleById;

public class RoleDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;

    public string? RoleDesc { get; set; }

    private class Mapping : AutoMapper.Profile
    {
        public Mapping()
        {
            CreateMap<AspNetRole, RoleDto>();
        }
    }
}
