using FitLog.Application.Common.Interfaces;
using FitLog.Application.Users.Queries.GetUsers;
using FitLog.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace FitLog.Application.Users.Queries.GetAccountByEmail;

public record GetAccountsByEmailQuery : IRequest<IEnumerable<AspNetUserListDTO>?>
{
    public string Email { get; set; } = string.Empty;
}

public class GetAccountsByEmailQueryValidator : AbstractValidator<GetAccountsByEmailQuery>
{
    public GetAccountsByEmailQueryValidator()
    {
    }
}

public class GetAccountsByEmailQueryHandler : IRequestHandler<GetAccountsByEmailQuery, IEnumerable<AspNetUserListDTO>?>
{
    private readonly UserManager<AspNetUser> _userManager;
    private readonly IMapper _mapper;

    public GetAccountsByEmailQueryHandler(UserManager<AspNetUser> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<IEnumerable<AspNetUserListDTO>?> Handle(GetAccountsByEmailQuery request, CancellationToken cancellationToken)
    {
        var users = await _userManager.Users
                .Where(u => (u.Email ?? "").Contains(request.Email))
                .ToListAsync(cancellationToken);

        if (!users.Any())
        {
            return null;
        }

        var userDtos = _mapper.Map<List<AspNetUserListDTO>>(users);
        return userDtos;
    }
}
