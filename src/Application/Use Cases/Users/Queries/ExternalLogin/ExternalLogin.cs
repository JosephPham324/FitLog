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
using System.Net.Http.Json;

namespace FitLog.Application.Users.Queries.ExternalLogin;

public record ExternalLoginQuery : IRequest<string>
{
    public string Provider { get; set; } = "";
    public string ReturnUrl { get; set; } = "";
    public string Token { get; set; } = "";
    public GoogleLoginRequest? GoogleLoginRequest { get; set; }
    public FacebookLoginRequest? FacebookLoginRequest { get; set; }
}

// GoogleLoginRequest.cs
public record GoogleLoginRequest
{
    public string Token { get; set; } = "";
}

public class FacebookLoginRequest
{
    public string UserId { get; set; } = "";
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
}

public class ExternalLoginQueryValidator : AbstractValidator<ExternalLoginQuery>
{
    public ExternalLoginQueryValidator()
    {
    }
}

public class ExternalLoginQueryHandler : IRequestHandler<ExternalLoginQuery, string>
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<AspNetUser> _userManager;

    public ExternalLoginQueryHandler(
        IConfiguration configuration,
        UserManager<AspNetUser> userManager)
    {
        _configuration = configuration;
        _userManager = userManager;
    }

    public async Task<string> Handle(ExternalLoginQuery request, CancellationToken cancellationToken)
    {
        switch (request.Provider)
        {
            case "Google":
                {
                    var token = request.GoogleLoginRequest != null? request.GoogleLoginRequest.Token : "";
                    var payload = await GoogleJsonWebSignature.ValidateAsync(token, new GoogleJsonWebSignature.ValidationSettings
                    {
                        Audience = new[] { _configuration["Authentication:Google:ClientId"] }
                    });

                    return await HandleUserLoginAsync(payload.Subject, payload.Email, "Google");
                }
            case "Facebook":
                {
                    var facebookInfo = request.FacebookLoginRequest ?? new FacebookLoginRequest();

                    return await HandleUserLoginAsync(facebookInfo.UserId, facebookInfo.Email, "Facebook");
                }
        }

        // If the provider is not supported, return an empty string or handle accordingly
        return "";
    }

    private async Task<string> HandleUserLoginAsync(string userId, string userEmail, string provider)
    {
        // Retrieve the user from the database
        var user = await _userManager.FindByEmailAsync(userEmail);
        if (user == null)
        {
            // Handle user not found, create a new user
            user = new AspNetUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = userEmail,
                Email = userEmail
            };


            if (provider == "Google")
            {
                user.GoogleID = userId;
            }
            else if (provider == "Facebook")
            {
                user.FacebookID = userId;
            }


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
        var jwtToken = await GenerateJwtTokenWithExternalProvider(user, userId);
        return jwtToken;
    }

    private async Task<string> GenerateJwtTokenWithExternalProvider(AspNetUser user, string providerUserId)
    {
        var claims = new List<Claim>
    {
        new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub, providerUserId),
        new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Email, user.Email ?? ""),
        new Claim("Id", user.Id),
        // Add other claims as needed
    };

        // Fetch roles and add them to claims
        var roles = await _userManager.GetRolesAsync(user);

        if (roles.IsNullOrEmpty())
        {
            await _userManager.AddToRoleAsync(user, "Member");
        }
        
        roles = await _userManager.GetRolesAsync(user);

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
            expires: DateTime.Now.AddMinutes(60),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

}
