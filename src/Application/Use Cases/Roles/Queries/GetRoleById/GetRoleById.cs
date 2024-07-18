using FitLog.Application.Common.Interfaces;
using FitLog.Domain.Entities;

namespace FitLog.Application.Roles.Queries.GetRoleById;

public record GetRoleByIdQuery : IRequest<RoleDto>
{
   public string RoleId { get; set; } = string.Empty;
}

public class GetRoleByIdQueryValidator : AbstractValidator<GetRoleByIdQuery>
{
    public GetRoleByIdQueryValidator()
    {
        RuleFor(v => v.RoleId)
            .NotEmpty();
    }
}

public class GetRoleByIdQueryHandler : IRequestHandler<GetRoleByIdQuery, RoleDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetRoleByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<RoleDto> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.AspNetRoles.FindAsync(request.RoleId);

        if (entity == null)
        {
            throw new NotFoundException(nameof(AspNetRole), request.RoleId + "");
            //return null; // Or throw an exception
        }

        return _mapper.Map<RoleDto>(entity);
    }
}
