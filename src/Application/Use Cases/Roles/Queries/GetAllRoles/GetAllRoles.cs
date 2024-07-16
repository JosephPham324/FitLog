using FitLog.Application.Common.Interfaces;
using FitLog.Application.Roles.Queries.GetRoleById;

namespace FitLog.Application.Roles.Queries.GetAllRoles;

public record GetAllRolesQuery : IRequest<List<RoleDto>>
{
}

public class GetAllRolesQueryValidator : AbstractValidator<GetAllRolesQuery>
{
    public GetAllRolesQueryValidator()
    {
    }
}

public class GetAllRolesQueryHandler : IRequestHandler<GetAllRolesQuery, List<RoleDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllRolesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<RoleDto>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
    {
        var entities = await _context.AspNetRoles.ToListAsync(cancellationToken);

        return _mapper.Map<List<RoleDto>>(entities);
    }
}
