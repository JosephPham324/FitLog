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
        var token = GetToken();

        if (string.IsNullOrEmpty(token))
        {
            return null;
        }

        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

        var userId = jsonToken?.Claims.FirstOrDefault(claim => claim.Type == "Id")?.Value;

        return userId;
    }

    private string? GetToken()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null)
        {
            throw new InvalidOperationException("HttpContext is null.");
        }
        // Try to get the token from the Authorization header
        var authorizationHeader = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();
        if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
        {
            return authorizationHeader.Substring("Bearer ".Length).Trim();
        }

        // If not found in the header, try to get it from the query string
        var token = _httpContextAccessor.HttpContext?.Request.Query["access_token"].ToString();
        if (!string.IsNullOrEmpty(token))
        {
            return token;
        }

        return null;
    }
}
