using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Models;
using FitLog.Application.Users.Queries.GetUsers;
using FitLog.Domain.Entities;
using Microsoft.AspNetCore.Identity;

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
    private readonly UserManager<AspNetUser> _userManager;

    public GetAccountByUsernameQueryHandler(IApplicationDbContext context, IMapper mapper, UserManager<AspNetUser> manager)
    {
        _context = context;
        _mapper = mapper;
        _userManager = manager;
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
        var dtos = _mapper.Map<List<UserListDTO>>(users);

        foreach (var userDto in dtos)
        {
            var user = await _userManager.FindByIdAsync(userDto.Id ?? "");
            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userDto.Roles = roles.ToList();
            }
        }

        return dtos;
    }
}
