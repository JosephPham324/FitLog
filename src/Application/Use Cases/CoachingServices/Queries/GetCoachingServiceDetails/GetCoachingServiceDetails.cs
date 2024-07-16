using FitLog.Application.Common.Interfaces;
using FitLog.Domain.Entities;

namespace FitLog.Application.CoachingServices.Queries.GetCoachingServiceDetails;

public record GetCoachingServiceDetailsQuery : IRequest<CoachingServiceDetailsDto>
{
    public int Id { get; init; }
}

public class GetCoachingServiceDetailsQueryValidator : AbstractValidator<GetCoachingServiceDetailsQuery>
{
    public GetCoachingServiceDetailsQueryValidator()
    {
        RuleFor(v => v.Id)
           .NotEmpty().WithMessage("Id is required.");
    }
}

public class GetCoachingServiceDetailsQueryHandler : IRequestHandler<GetCoachingServiceDetailsQuery, CoachingServiceDetailsDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCoachingServiceDetailsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CoachingServiceDetailsDto> Handle(GetCoachingServiceDetailsQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.CoachingServices
            .FirstOrDefaultAsync(cs => cs.Id == request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(CoachingService), request.Id + "");
        }
        var createdByUser = await _context.AspNetUsers
                   .FirstOrDefaultAsync(u => u.Id == entity.CreatedBy, cancellationToken);

        var lastModifiedByUser = await _context.AspNetUsers
            .FirstOrDefaultAsync(u => u.Id == entity.LastModifiedBy, cancellationToken);

        var coachingServiceDto = _mapper.Map<CoachingServiceDetailsDto>(entity);
        coachingServiceDto.CreatedByUserName = createdByUser?.UserName;
        coachingServiceDto.LastModifiedByUserName = lastModifiedByUser?.UserName;

        return coachingServiceDto;
    }
}
