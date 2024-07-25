using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using FitLog.Application.Common.Interfaces;

namespace FitLog.Web.Services;

public class CurrentUser : IUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }


    public string? Id => _httpContextAccessor.HttpContext?.User?.FindFirstValue("Id");
}
