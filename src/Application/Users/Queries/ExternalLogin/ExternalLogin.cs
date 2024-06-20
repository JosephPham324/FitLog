using FitLog.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.JsonWebTokens;
using Google.Apis.Auth;
using FitLog.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace FitLog.Application.Users.Queries.ExternalLogin;

public record ExternalLoginQuery : IRequest<string>
{
    public string Provider { get; set; } = "";
    public string ReturnUrl { get; set; } = "";
    public string Token { get; set; } = "";
}

// GoogleLoginRequest.cs
public record GoogleLoginRequest
{
    public string Token { get; set; } = "";
}

public class ExternalLoginQueryValidator : AbstractValidator<ExternalLoginQuery>
{
    public ExternalLoginQueryValidator()
    {
    }
}

public class ExternalLoginQueryHandler : IRequestHandler<ExternalLoginQuery, string>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;
    private readonly UserManager<AspNetUser> _userManager;


    public ExternalLoginQueryHandler(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, UserManager<AspNetUser> userManager)
    {
        _httpContextAccessor = httpContextAccessor;
        _configuration = configuration;
        _userManager = userManager;
    }

    public async Task<string> Handle(ExternalLoginQuery request, CancellationToken cancellationToken)
    {
        switch (request.Provider)
        {
            case "Google":
                {
                    var payload = await GoogleJsonWebSignature.ValidateAsync(request.Token, new GoogleJsonWebSignature.ValidationSettings
                    {
                        // Get from appsettings
                        Audience = new[] { _configuration["Authentication:Google:ClientId"] }
                    });

                    // Retrieve the user from the database
                    var user = await _userManager.FindByEmailAsync(payload.Email);
                    if (user == null)
                    {
                        // Handle user not found, create a new user
                        user = new AspNetUser
                        {
                            Id = Guid.NewGuid().ToString(),
                            UserName = payload.Email,
                            Email = payload.Email
                        };

                        var result = await _userManager.CreateAsync(user);
                        if (!result.Succeeded)
                        {
                            // Handle user creation failure (e.g., return an error message)
                            return "";
                        }

                        // Optionally, assign a default role to the new user
                        await _userManager.AddToRoleAsync(user, "Member");
                    }

                    // Generate your own JWT token
                    var jwtToken = await GenerateJwtToken(user, payload);
                    return jwtToken;
                }
        };

        // If the provider is not supported, return an empty string or handle accordingly
        return "";
    }

    private async Task<string> GenerateJwtToken(AspNetUser user, GoogleJsonWebSignature.Payload payload)
    {
        var claims = new List<Claim>
        {
            new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub, payload.Subject),
            new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Email, payload.Email),
            new Claim("Id", user.Id),
            // Add other claims as needed
        };

        // Fetch roles and add them to claims
        var roles = await _userManager.GetRolesAsync(user);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? ""));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"] ?? "",
            audience: _configuration["Jwt:Audience"] ?? "",
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
