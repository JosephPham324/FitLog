using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Mappings;
using FitLog.Application.Common.Models;
using FitLog.Domain.Entities;

namespace FitLog.Application.Users.Queries.GetUsers;

public record GetUsersListWithPaginationRequest : IRequest<PaginatedList<AspNetUserListDTO>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}
public class GetUsersListWithPaginationQueryHandler : IRequestHandler<GetUsersListWithPaginationRequest, PaginatedList<AspNetUserListDTO>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetUsersListWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<AspNetUserListDTO>> Handle(GetUsersListWithPaginationRequest request, CancellationToken cancellationToken)
    {
        return await _context.AspNetUsers
                    .Where(user=>user.IsDeleted == null || user.IsDeleted == null && user.IsDeleted == false )
                    .ProjectTo<AspNetUserListDTO>(_mapper.ConfigurationProvider)
                    .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
