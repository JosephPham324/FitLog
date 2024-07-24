using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Mappings;
using FitLog.Application.Common.Models;
using FitLog.Domain.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FitLog.Application.Users.Queries.GetUsers
{
    public record GetUsersListWithPaginationRequest : IRequest<PaginatedList<UserListDTO>>
    {
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }

    public class GetUsersListWithPaginationQueryHandler : IRequestHandler<GetUsersListWithPaginationRequest, PaginatedList<UserListDTO>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<AspNetUser> _userManager;

        public GetUsersListWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper, UserManager<AspNetUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<PaginatedList<UserListDTO>> Handle(GetUsersListWithPaginationRequest request, CancellationToken cancellationToken)
        {
            var usersQuery = _context.AspNetUsers
                .ProjectTo<UserListDTO>(_mapper.ConfigurationProvider)
                .AsQueryable();

            var paginatedList = await usersQuery.PaginatedListAsync(request.PageNumber, request.PageSize);

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
}
