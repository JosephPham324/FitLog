
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using FitLog.Application.Common.Models;
using FitLog.Application.Users.Commands.Login;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


namespace FitLog.Web.Endpoints;

public class Login : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        //app.MapGroup(this)
        //    .MapPost
        throw new NotImplementedException();
    }

    public Task<string> LoginEndpoint(ISender sender, [AsParameters] LoginCommand command)
    {
        return sender.Send(command);
    }

}
