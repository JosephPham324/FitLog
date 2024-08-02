using FitLog.Application.Common.Interfaces;
using FitLog.Application.Users.Queries.GetUsers;
using FitLog.Domain.Entities;
using AutoMapper;
using MediatR;
using FitLog.Application.Common.Models;
using FitLog.Application.Common.Mappings;
using Microsoft.AspNetCore.Identity;

public record SearchUsersWithPaginationQuery : IRequest<PaginatedList<UserListDTO>>
{
    public string? Email { get; init; }
    public string? Provider { get; init; } // "Google" or "Facebook"
    public string? Username { get; init; }
    public string? Roles { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class SearchUsersWithPaginationQueryValidator : AbstractValidator<SearchUsersWithPaginationQuery>
{
    public SearchUsersWithPaginationQueryValidator()
    {
        RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1).WithMessage("Page number must be at least 1.");
        RuleFor(x => x.PageSize).GreaterThanOrEqualTo(1).WithMessage("Page size must be at least 1.");
    }
}

public class SearchUsersWithPaginationQueryHandler : IRequestHandler<SearchUsersWithPaginationQuery, PaginatedList<UserListDTO>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly UserManager<AspNetUser> _userManager;

    public SearchUsersWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper, UserManager<AspNetUser> userManager)
    {
        _context = context;
        _mapper = mapper;
        _userManager = userManager;
    }

    public async Task<PaginatedList<UserListDTO>> Handle(SearchUsersWithPaginationQuery request, CancellationToken cancellationToken)
    {
        IQueryable<AspNetUser> query = _context.AspNetUsers;

        if (!string.IsNullOrEmpty(request.Email))
        {
            query = query.Where(u => EF.Functions.Like(u.Email, $"%{request.Email}%"));
        }

        if (!string.IsNullOrEmpty(request.Provider))
        {
            if (request.Provider == "Google")
            {
                query = query.Where(u => u.GoogleID != null);
            }
            else if (request.Provider == "Facebook")
            {
                query = query.Where(u => u.FacebookID != null);
            }
        }

        if (!string.IsNullOrEmpty(request.Username))
        {
            query = query.Where(u => EF.Functions.Like(u.UserName, $"%{request.Username}%"));
        }

        if (!string.IsNullOrEmpty(request.Roles))
        {
            var roles = request.Roles.Split(',').Select(role => role.Trim()).ToList();
            var usersInRoles = new List<AspNetUser>();

            foreach (var role in roles)
            {
                var usersInRole = await _userManager.GetUsersInRoleAsync(role);
                usersInRoles.AddRange(usersInRole);
            }

            var distinctUsers = usersInRoles.DistinctBy(user => user.Id).Select(user => user.Id);
            query = query.Where(user => distinctUsers.Contains(user.Id));
        }

        var paginatedList = await query
            .ProjectTo<UserListDTO>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);

        foreach (var userDto in paginatedList.Items)
        {
            var user = await _userManager.FindByIdAsync(userDto.Id ?? "");
            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userDto.Roles = roles.ToList();
            }
        }

        return paginatedList;
    }
}
