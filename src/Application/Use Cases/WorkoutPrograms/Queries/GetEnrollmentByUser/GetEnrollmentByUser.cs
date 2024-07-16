using FitLog.Application.Common.Interfaces;
using FitLog.Application.Use_Cases.WorkoutPrograms.DTOs;

namespace FitLog.Application.WorkoutPrograms.Queries.GetEnrollmentByUser;

public record GetEnrollmentsByUserQuery : IRequest<List<ProgramEnrollmentDTO>>
{
    public string UserId { get; init; } = string.Empty;
}


public class GetEnrollmentByUserQueryValidator : AbstractValidator<GetEnrollmentsByUserQuery>
{
    public GetEnrollmentByUserQueryValidator()
    {
    }
}

public class GetEnrollmentByUserQueryHandler : IRequestHandler<GetEnrollmentsByUserQuery, List<ProgramEnrollmentDTO>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetEnrollmentByUserQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ProgramEnrollmentDTO>> Handle(GetEnrollmentsByUserQuery request, CancellationToken cancellationToken)
    {
        var enrollments = await _context.ProgramEnrollments
            .Where(pe => pe.UserId == request.UserId)
            .Include(pe => pe.Program)
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<ProgramEnrollmentDTO>>(enrollments);
    }
}
