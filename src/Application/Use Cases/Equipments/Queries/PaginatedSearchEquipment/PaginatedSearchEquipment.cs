using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Mappings;
using FitLog.Application.Common.Models;
using FitLog.Application.Equipments.Queries.GetEquipmentsList;

namespace FitLog.Application.Equipments.Queries.PaginatedSearchEquipment;

public record PaginatedSearchEquipmentQuery : IRequest<PaginatedList<EquipmentDetailsDTO>>
{
    public string? EquipmentName { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class PaginatedSearchEquipmentQueryValidator : AbstractValidator<PaginatedSearchEquipmentQuery>
{
    public PaginatedSearchEquipmentQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page number must be at least 1.");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page size must be at least 1.");
    }
}

public class PaginatedSearchEquipmentQueryHandler : IRequestHandler<PaginatedSearchEquipmentQuery, PaginatedList<EquipmentDetailsDTO>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public PaginatedSearchEquipmentQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<EquipmentDetailsDTO>> Handle(PaginatedSearchEquipmentQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Equipment.AsQueryable();

        if (!string.IsNullOrEmpty(request.EquipmentName))
        {
            query = query.Where(e => EF.Functions.Like(e.EquipmentName, $"%{request.EquipmentName}%"));
        }

        return await query.OrderBy(e => e.EquipmentName)
                          .ProjectTo<EquipmentDetailsDTO>(_mapper.ConfigurationProvider)
                          .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
