using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FitLog.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace FitLog.Application.Users.Queries.ExternalLoginCallback;

public record ExternalLoginCallbackQuery : IRequest<string>
{
}

public class ExternalLoginCallbackQueryValidator : AbstractValidator<ExternalLoginCallbackQuery>
{
    public ExternalLoginCallbackQueryValidator()
    {
    }
}

public class ExternalLoginCallbackQueryHandler : IRequestHandler<ExternalLoginCallbackQuery, string>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;

    public ExternalLoginCallbackQueryHandler(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
    {
        _httpContextAccessor = httpContextAccessor;
        _configuration = configuration;
    }

    public async Task<string> Handle(ExternalLoginCallbackQuery request, CancellationToken cancellationToken)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        string token = "";
        if (httpContext != null)
        {
            var authenticateResult = await httpContext.AuthenticateAsync(JwtBearerDefaults.AuthenticationScheme);
            if (!authenticateResult.Succeeded)
                throw new Exception("External authentication failed.");

            var claims = authenticateResult.Principal.Identities
            .FirstOrDefault()?.Claims;


            if (claims!= null) token = GenerateJwtToken(claims);
            return token;
        }
        return "";
    }

    private string GenerateJwtToken(IEnumerable<Claim> claims)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ??""));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims.Select(c => new Claim(c.Type, c.Value.ToString()))),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = credentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
