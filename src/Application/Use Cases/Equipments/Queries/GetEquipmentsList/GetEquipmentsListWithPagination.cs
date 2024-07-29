using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Mappings;
using FitLog.Application.Common.Models;
using FitLog.Application.TodoLists.Queries.GetTodos;

namespace FitLog.Application.Equipments.Queries.GetEquipmentsList;

public record GetEquipmentsWithPaginationQuery : IRequest<PaginatedList<EquipmentDetailsDTO>>
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

public class GetEquipmentsListWithPaginationQueryHandler : IRequestHandler<GetEquipmentsWithPaginationQuery, PaginatedList<EquipmentDetailsDTO>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetEquipmentsListWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<EquipmentDetailsDTO>> Handle(GetEquipmentsWithPaginationQuery request, CancellationToken cancellationToken)
    {
         return await _context.Equipment
                 .AsNoTracking()
                 .ProjectTo<EquipmentDetailsDTO>(_mapper.ConfigurationProvider)
                 .OrderBy(t => t.EquipmentId)
                 .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
