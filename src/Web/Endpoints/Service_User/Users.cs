
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using FitLog.Application.Common.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using FitLog.Application.Users.Commands.Regiser;
using FitLog.Application.Users.Commands.Register;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using FitLog.Application.Common.Security;
using FitLog.Application.Users.Queries.GetUserDetails;
using FitLog.Application.Users.Queries.Login;
using FitLog.Application.Users.Queries.GetUsers;
using FitLog.Application.Users.Queries.GetAccountByEmail;
using FitLog.Application.Users.Queries.GetAccountByExternalProvider;

namespace FitLog.Web.Endpoints.Service_User;

public class Users : EndpointGroupBase
{

    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapPost(Login, "login")
            .MapPost(Register, "register")
            .MapGet(GetUserList, "all")
            .MapGet(SearchUsersByEmail, "search-by-email")
            .MapGet(SearchUsersByLoginProvider,"search-by-provider")
            .MapGet(GetUserProfile, "profile");
    }

    /// <summary>
    /// Logs in a user using the provided login query parameters.
    /// </summary>
    /// <param name="sender">The sender used to send the login query.</param>
    /// <param name="query">The login query containing the user's login information.</param>
    /// <returns>A task that represents the asynchronous login operation. The task result contains the login result DTO.</returns>
    public Task<LoginResultDTO> Login(ISender sender, [AsParameters] LoginQuery query)
    {
        return sender.Send(query);
    }

    /// <summary>
    /// Registers a new user using the provided register command parameters.
    /// </summary>
    /// <param name="sender">The sender used to send the register command.</param>
    /// <param name="command">The register command containing the user's registration information.</param>
    /// <returns>A task that represents the asynchronous register operation. The task result contains the register result DTO.</returns>
    public Task<RegisterResultDTO> Register(ISender sender, [FromBody] RegisterCommand command)
    {
        return sender.Send(command);
    }

    /// <summary>
    /// Gets a paginated list of users based on the provided pagination request parameters.
    /// </summary>
    /// <param name="sender">The sender used to send the get users list request.</param>
    /// <param name="request">The request containing pagination parameters for fetching the users list.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the paginated list of users.</returns>
    //[Authorize(Roles ="Admin")]
    //public Task<List<UserDTO>>
    public Task<PaginatedList<AspNetUserListDTO>> GetUserList(ISender sender, [AsParameters] GetUsersListWithPaginationRequest request)
    {
        return sender.Send(request);
    }

    /// <summary>
    /// Retrieves the user profile details based on the provided request parameters.
    /// </summary>
    /// <param name="sender">The sender used to send the get profile details request.</param>
    /// <param name="request">The request containing parameters for fetching the user profile details.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the user profile DTO.</returns>
    public Task<UserProfileDTO> GetUserProfile(ISender sender, [AsParameters] GetProfileDetailsRequest request)
    {
        return sender.Send(request);
    }

    /// <summary>
    /// Gets a paginated list of users based on the provided pagination request parameters.
    /// </summary>
    /// <param name="sender">The sender used to send the get users list request.</param>
    /// <param name="request">The request containing pagination parameters for fetching the users list.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the paginated list of users.</returns>
    //[Authorize(Roles ="Admin")]
    //public Task<List<UserDTO>>
    public Task<IEnumerable<AspNetUserListDTO>?> SearchUsersByEmail(ISender sender, [AsParameters] GetAccountsByEmailQuery request)
    {
        return sender.Send(request);
    }

    public Task<IEnumerable<AspNetUserListDTO>?> SearchUsersByLoginProvider(ISender sender, [AsParameters] GetAccountByExternalProviderQuery request)
    {
        return sender.Send(request);
    }
}
