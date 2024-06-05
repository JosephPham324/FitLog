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

namespace FitLog.Application.Users.Commands.Login;
public record LoginCommand : IRequest<string>
{
    public string? Username { get; init; }
    public string? Password { get; init; }
}

public class Login : IRequestHandler<LoginCommand, string>
{
    private readonly IApplicationDbContext _context;

    public string GenerateJwtToken(string email, AspNetUser user, IConfiguration configuration)
    {
        var issuer = configuration["Jwt:Issuer"];
        var audience = configuration["Jwt:Audience"];
        var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]??"");
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("Id", Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName??""),
                new Claim(JwtRegisteredClaimNames.Email, user.UserName??""),
                new Claim(JwtRegisteredClaimNames.Jti,
                Guid.NewGuid().ToString())
            }),
            Expires = DateTime.UtcNow.AddMinutes(5),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials
            (new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha512Signature)
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = tokenHandler.WriteToken(token);
        //var stringToken = tokenHandler.WriteToken(token);

        return jwtToken;
    }
    public Login(IApplicationDbContext context)
    {
        _context = context;
    }

    public Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
    {

        throw new NotImplementedException();
    }
}
