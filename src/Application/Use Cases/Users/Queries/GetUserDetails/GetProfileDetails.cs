using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Models;
using FitLog.Application.Users.Queries.GetUsers;
using FitLog.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace FitLog.Application.Users.Queries.GetUserDetails;

public record GetProfileDetailsRequest : IRequest<UserProfileDTO>
{
    public string UserId { get; set; } = "";
}
public class GetProfileDetailsQueryHandler : IRequestHandler<GetProfileDetailsRequest, UserProfileDTO>
{
    private readonly IApplicationDbContext _context;
    private readonly UserManager<AspNetUser> _userManager;
    private readonly IMapper _mapper;

    public GetProfileDetailsQueryHandler(IApplicationDbContext context, UserManager<AspNetUser> userManager, IMapper mapper)
    {
        _context = context;
        _userManager = userManager;
        _mapper = mapper;
    }
    public async Task<UserProfileDTO> Handle(GetProfileDetailsRequest request, CancellationToken cancellationToken)
    {
        var user = await _context.AspNetUsers
            .Include(u => u.Programs)
            .Include(u => u.Certifications)
            .Include(u => u.CoachingServices)
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user == null)
        {
            throw new NotFoundException(nameof(AspNetUser), request.UserId);
        }


        var userProfileDto = _mapper.Map<UserProfileDTO>(user);
        var roles = await _userManager.GetRolesAsync(user);
        userProfileDto.Roles = string.Join(", ", roles);

        return userProfileDto;
    }

}
