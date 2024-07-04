using FitLog.Application.Common.Interfaces;
using FitLog.Domain.Constants;

namespace FitLog.Application.Exercises.Queries.GetExercsieTypes;

public record GetExercsieTypesQuery : IRequest<IEnumerable<string?>>;

public class GetExercsieTypesQueryValidator : AbstractValidator<GetExercsieTypesQuery>
{
    public GetExercsieTypesQueryValidator()
    {
    }
}

public class GetExercsieTypesQueryHandler : IRequestHandler<GetExercsieTypesQuery, IEnumerable<string?>>
{
    private readonly IApplicationDbContext _context;

    public GetExercsieTypesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public Task<IEnumerable<string?>> Handle(GetExercsieTypesQuery request, CancellationToken cancellationToken)
    {
        var exerciseTypes = typeof(ExerciseTypes)
            .GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.FlattenHierarchy)
            .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.FieldType == typeof(string))
            .Select(fi => fi.GetRawConstantValue() as string);

        return Task.FromResult<IEnumerable<string?>>(exerciseTypes);
    }
}
