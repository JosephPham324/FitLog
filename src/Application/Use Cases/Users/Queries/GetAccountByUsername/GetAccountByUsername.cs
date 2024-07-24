using FitLog.Application.Common.Interfaces;
using FitLog.Application.Users.Queries.GetUsers;

namespace FitLog.Application.Users.Queries.GetAccountByUsername;

public record GetAccountByUsernameQuery : IRequest<IEnumerable<UserListDTO>?>
{
    public string Username { get; set; } = string.Empty;
}


public class GetAccountByUsernameQueryValidator : AbstractValidator<GetAccountByUsernameQuery>
{
    public GetAccountByUsernameQueryValidator()
    {
        RuleFor(x => x.Username).NotEmpty().WithMessage("Username is required.");
    }
}


public class GetAccountByUsernameQueryHandler : IRequestHandler<GetAccountByUsernameQuery, IEnumerable<UserListDTO>?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAccountByUsernameQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UserListDTO>?> Handle(GetAccountByUsernameQuery request, CancellationToken cancellationToken)
    {
        var users = await _context.AspNetUsers
            .Where(u => EF.Functions.Like(u.UserName, $"%{request.Username}%"))
            .ToListAsync(cancellationToken);

        if (users == null || users.Count == 0)
        {
            return new List<UserListDTO>(); // Return an empty list if no users found
        }

        return _mapper.Map<List<UserListDTO>>(users);
    }
}
