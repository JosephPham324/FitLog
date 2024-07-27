using FitLog.Application.Common.Interfaces;
using FitLog.Application.Users.Queries.GetUsers;
using FitLog.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace FitLog.Application.Users.Queries.GetAccountByEmail;

public record GetAccountsByEmailQuery : IRequest<IEnumerable<UserListDTO>?>
{
    public string Email { get; set; } = string.Empty;
}

public class GetAccountsByEmailQueryValidator : AbstractValidator<GetAccountsByEmailQuery>
{
    public GetAccountsByEmailQueryValidator()
    {
    }
}

public class GetAccountsByEmailQueryHandler : IRequestHandler<GetAccountsByEmailQuery, IEnumerable<UserListDTO>?>
{
    private readonly UserManager<AspNetUser> _userManager;
    private readonly IMapper _mapper;

    public GetAccountsByEmailQueryHandler(UserManager<AspNetUser> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UserListDTO>?> Handle(GetAccountsByEmailQuery request, CancellationToken cancellationToken)
    {
        var users = await _userManager.Users

                .Where(u => (u.Email ?? "").Contains(request.Email))
                .ToListAsync(cancellationToken);

        if (!users.Any())
        {
            return null;
        }

        var userDtos = _mapper.Map<List<UserListDTO>>(users);
        return userDtos;
    }
}
