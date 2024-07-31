using FitLog.Application.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using FluentValidation;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FitLog.Application.Users.Queries.GetUsers;
using FitLog.Domain.Entities;

namespace FitLog.Application.Users.Queries.GetAccountsByRole
{
    public record GetAccountsByRoleQuery : IRequest<IEnumerable<UserListDTO>>
    {
        public string Roles { get; init; } = string.Empty;
    }

    public class GetAccountsByRoleQueryValidator : AbstractValidator<GetAccountsByRoleQuery>
    {
        public GetAccountsByRoleQueryValidator()
        {
            RuleFor(x => x.Roles).NotEmpty().WithMessage("At least one role is required.");
        }
    }

    public class GetAccountsByRoleQueryHandler : IRequestHandler<GetAccountsByRoleQuery, IEnumerable<UserListDTO>>
    {
        private readonly UserManager<AspNetUser> _userManager;
        private readonly IMapper _mapper;

        public GetAccountsByRoleQueryHandler(UserManager<AspNetUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserListDTO>> Handle(GetAccountsByRoleQuery request, CancellationToken cancellationToken)
        {
            var roles = request.Roles.Split(',').Select(role => role.Trim()).ToList();
            var usersInRoles = new List<AspNetUser>();

            foreach (var role in roles)
            {
                var usersInRole = await _userManager.GetUsersInRoleAsync(role);
                usersInRoles.AddRange(usersInRole);
            }

            var distinctUsers = usersInRoles.DistinctBy(user => user.Id).ToList(); // Ensure unique users
            var userDtos = _mapper.Map<List<UserListDTO>>(distinctUsers);

            return userDtos;
        }
    }
}
