using FitLog.Application.Common.Interfaces;
using FitLog.Application.Equipments.Queries.GetEquipmentsList;
using FitLog.Domain.Entities;

namespace FitLog.Application.Equipments.Queries.GetEquipmentDetails;

public record GetEquipmentDetailsQuery : IRequest<EquipmentDetailsDTO>
{
    public int EquipmentId { get; init; }

}

public class GetEquipmentDetailsQueryValidator : AbstractValidator<GetEquipmentDetailsQuery>
{
    public GetEquipmentDetailsQueryValidator()
    {
    }
}

public class GetEquipmentDetailsQueryHandler : IRequestHandler<GetEquipmentDetailsQuery, EquipmentDetailsDTO>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetEquipmentDetailsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<EquipmentDetailsDTO> Handle(GetEquipmentDetailsQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.Equipment
            .ProjectTo<EquipmentDetailsDTO>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(e => e.EquipmentId == request.EquipmentId, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Equipment), request.EquipmentId + "");
        }

        return entity;
    }
}
