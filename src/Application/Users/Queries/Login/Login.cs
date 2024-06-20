using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FitLog.Application.Common.Interfaces;
using FitLog.Application.TodoLists.Commands.CreateTodoList;
using FitLog.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace FitLog.Application.Users.Queries.Login;
public record LoginQuery : IRequest<LoginResultDTO>
{
    public string Username { get; init; } = "";
    public string Password { get; init; } = "";
}

public class LoginHandler : IRequestHandler<LoginQuery, LoginResultDTO>
{
    //private readonly IApplicationDbContext _context;
    private readonly UserManager<AspNetUser> _userManager;
    private readonly IConfiguration _configuration;
    public LoginHandler(UserManager<AspNetUser> userManager, IConfiguration configuration)
    {
        //_context = context;
        _userManager = userManager;
        _configuration = configuration;
    }

    /// <summary>
    /// Generates a JWT token for a validated user.
    /// </summary>
    /// <param name="email">The email of the user (used for the token).</param>
    /// <param name="user">The user object for whom the token is being generated.</param>
    /// <param name="configuration">Configuration instance to access JWT settings.</param>
    /// <returns>A JWT string token.</returns>
    private async Task<string> GenerateJwtToken(string email, AspNetUser user, IConfiguration configuration)
    {
        var issuer = configuration["Jwt:Issuer"];
        var audience = configuration["Jwt:Audience"];
        var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"] ?? "");

        var claims = new List<Claim>
    {
        new Claim("Id", Guid.NewGuid().ToString()),
        new Claim(JwtRegisteredClaimNames.Sub, user.UserName ?? ""),
        new Claim(JwtRegisteredClaimNames.Email, user.UserName ?? ""),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

        // Fetch roles and add them to claims
        var roles = await _userManager.GetRolesAsync(user);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(5),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = tokenHandler.WriteToken(token);

        return jwtToken;
    }

    /// <summary>
    /// Handles the login request command by validating user credentials and generating a JWT token.
    /// </summary>
    /// <param name="request">Login command containing username and password.</param>
    /// <param name="cancellationToken">Token for operation cancellation.</param>
    /// <returns>A LoginResultDTO containing the operation result and the JWT token if successful.</returns>
    public async Task<LoginResultDTO> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.Username);
        if (user != null && await _userManager.CheckPasswordAsync(user, request.Password))
        {
            var token = await GenerateJwtToken(request.Username, user, _configuration);
            return new LoginResultDTO { Success = true, Token = token };
        }

        return new LoginResultDTO { Success = false };
    }
}
