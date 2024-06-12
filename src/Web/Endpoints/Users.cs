
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using FitLog.Application.Common.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using FitLog.Application.Users.Queries_.Login;
using FitLog.Application.Users.Commands.Regiser;
using FitLog.Application.Users.Commands.Register;
using FitLog.Application.Users.Queries_.GetUsers;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using FitLog.Application.Common.Security;


namespace FitLog.Web.Endpoints;

public class Users: EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapPost(Login, "login")
            .MapPost(Register,"register")
            .MapGet(GetUserList,"get-all");
    }

    public Task<LoginResultDTO> Login(ISender sender, [AsParameters] LoginQuery query)
    {
        return sender.Send(query);
    }

    public Task<RegisterResultDTO> Register(ISender sender, [AsParameters] RegisterCommand command)
    {
        return sender.Send(command);
    }

    //[Authorize(Roles ="Admin")]
    //public Task<List<UserDTO>>
    public Task<PaginatedList<AspNetUserListDTO>> GetUserList(ISender sender, [AsParameters] GetUsersListWithPaginationRequest request)
    {
        return sender.Send(request);
    }
}
