using FitLog.Application.Common.Interfaces;
using FitLog.Application.Users.Queries.GetUsers;
using FitLog.Domain.Entities;
using FitLog.Domain.Constants;
using System.Reflection;


namespace FitLog.Application.Users.Queries.GetAccountByExternalProvider;

public record GetAccountByExternalProviderQuery : IRequest<IEnumerable<UserListDTO>?>
{
    public string Provider { get; set; } = string.Empty; // "Google" or "Facebook"
}

public class GetAccountByExternalProviderQueryValidator : AbstractValidator<GetAccountByExternalProviderQuery>
{
    public GetAccountByExternalProviderQueryValidator()
    {
        var providers = typeof(ExternalLoginProviders)
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.FieldType == typeof(string))
                .Select(fi => fi.GetRawConstantValue() as string)
                .ToList();

        RuleFor(x => x.Provider)
               .NotEmpty().WithMessage("Provider must not be empty.")
               .Must(provider => providers.Contains(provider))
               .WithMessage($"Provider must be one of the following: {string.Join(", ", providers)}");

    }
}

public class GetAccountByExternalProviderQueryHandler : IRequestHandler<GetAccountByExternalProviderQuery, IEnumerable<UserListDTO>?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAccountByExternalProviderQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UserListDTO>?> Handle(GetAccountByExternalProviderQuery request, CancellationToken cancellationToken)
    {
        IQueryable<AspNetUser> query = _context.AspNetUsers;

        if (request.Provider == "Google")
        {
            query = query.Where(u => u.GoogleID != null);
        }
        else if (request.Provider == "Facebook")
        {
            query = query.Where(u => u.FacebookID != null);
        }
        else
        {
            return Enumerable.Empty<UserListDTO>();
        }

        var users = await query.ToListAsync(cancellationToken);
        var userDtos = _mapper.Map<List<UserListDTO>>(users);
        return userDtos;
    }
}
