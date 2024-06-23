using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using FitLog.Application.Common.Interfaces;

namespace FitLog.Web.Services;

public class CurrentUserFromToken : IUserTokenService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserFromToken(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? GetUserIdFromGivenToken(string token)
    {
        if (string.IsNullOrEmpty(token))
        {
            return null;
        }

        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

        var userId = jsonToken?.Claims.FirstOrDefault(claim => claim.Type == "Id")?.Value;

        return userId;
    }

    public string? GetUserIdFromToken()
    {
        var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

        if (string.IsNullOrEmpty(token))
        {
            return null;
        }

        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

        var userId = jsonToken?.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;

        return userId;
    }
}
