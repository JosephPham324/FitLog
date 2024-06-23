using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Models;

namespace FitLog.Application.Equipments.Queries.GetEquipmentsList;

public record GetEquipmentsWithPaginationQuery : IRequest<PaginatedList<EquipmentDTO>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetEquipmentsListWithPaginationQueryValidator : AbstractValidator<GetEquipmentsWithPaginationQuery>
{
    public GetEquipmentsListWithPaginationQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page number must be at least 1.");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page size must be at least 1.");
    }
}

public class GetEquipmentsListWithPaginationQueryHandler : IRequestHandler<GetEquipmentsWithPaginationQuery, PaginatedList<EquipmentDTO>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetEquipmentsListWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<EquipmentDTO>> Handle(GetEquipmentsWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Equipment
            .OrderBy(e => e.EquipmentName)
            .ProjectTo<EquipmentDTO>(_mapper.ConfigurationProvider);
        
        return await PaginatedList<EquipmentDTO>.CreateAsync(query.AsNoTracking(), request.PageNumber, request.PageSize);
    }
}
