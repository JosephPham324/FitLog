using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using FitLog.Application.Common.Models;
using FitLog.Application.Users.Commands.Register;
using FitLog.Application.Users.Queries.GetUserDetails;
using FitLog.Application.Users.Queries.Login;
using FitLog.Application.Users.Queries.GetUsers;
using FitLog.Application.Users.Queries.GetAccountByEmail;
using FitLog.Application.Users.Queries.GetAccountByExternalProvider;
using FitLog.Application.Users.Queries.GetAccountByUsername;
using FitLog.Application.Users.Commands.CreateUser;
using FitLog.Application.Users.Commands.DeleteAccount;
using FitLog.Application.Users.Commands.RecoverAccount;
using FitLog.Application.Users.Commands.UpdateUser;
using FitLog.Application.Users.Queries.GetCoachesListWithPagination;
using FitLog.Application.Users.Commands.ResetPassword;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using FitLog.Application.Users.Commands.ConfirmEmail;
using FitLog.Application.Common.Interfaces;
using FitLog.Web.Services;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using FitLog.Application.Users.Queries.GetAccountsByRole;

namespace FitLog.Web.Endpoints.Service_User;

public class Users : EndpointGroupBase
{
    private readonly IUserTokenService _tokenService;
    private readonly IUser _identityService;

    public Users()
    {
        _tokenService = new CurrentUserFromToken(httpContextAccessor: new HttpContextAccessor());
        _identityService = new CurrentUser(httpContextAccessor: new HttpContextAccessor());
    }
    public override void Map(WebApplication app)
    {
        var coachGroup = app.MapGroup(this).MapGroup("/coaches");

        // Standalones
        app.MapGroup(this)
           .MapPost(Register, "register")
           .MapPut(ConfirmEmail, "confirm-email")
           .MapPost(RecoverAccount, "recover-account")
           .MapPut(ResetPassword, "reset-password");

        app.MapGroup(this)
           //.RequireAuthorization()
           .MapGet(GetUserProfile, "user-profile")
           .MapPut(AuthenticatedResetPassword, "authenticated-reset-password")
           .MapPut(UpdateUserProfile, "update-profile");


        // User management routes
        app.MapGroup(this)
           //.RequireAuthorization("AdminOnly")
           .MapGet(GetUserList, "all")
           .MapGet(SearchUsers, "search")
           .MapGet(SearchUsersByEmail, "search-by-email")
           .MapGet(SearchUsersByLoginProvider, "search-by-provider")
           .MapGet(SearchUsersByUserName, "search-by-username")
           .MapGet(GetUsersByRole, "get-by-roles")
           .MapPost(CreateUser, "create-account")
           .MapDelete(DeleteAccount, "delete-account/{id}");
        // Coaches routes
        app.MapGroup(this)
           .MapGroup("/coaches")
           .MapGet(GetCoachesList, "coaches");


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
    public Task<Result> Register(ISender sender, [FromBody] RegisterCommand command)
    {
        return sender.Send(command);
    }

    /// <summary>
    /// Gets a paginated list of users based on the provided pagination request parameters.
    /// </summary>
    /// <param name="sender">The sender used to send the get users list request.</param>
    /// <param name="request">The request containing pagination parameters for fetching the users list.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the paginated list of users.</returns>
    public Task<PaginatedList<UserListDTO>> GetUserList(ISender sender, [AsParameters] GetUsersListWithPaginationRequest request)
    {
        return sender.Send(request);
    }

    /// <summary>
    /// Retrieves the user profile details based on the provided request parameters.
    /// </summary>
    /// <param name="sender">The sender used to send the get profile details request.</param>
    /// <param name="request">The request containing parameters for fetching the user profile details.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the user profile DTO.</returns>
    public Task<UserProfileDTO> GetUserProfile(ISender sender /*[AsParameters] GetProfileDetailsRequest request*/)
    {

        var UserId = _identityService.Id ?? "";
        var request = new GetProfileDetailsRequest()
        {
            UserId = UserId
        };
        return sender.Send(request);
    }

    public Task<PaginatedList<UserListDTO>> SearchUsers(ISender sender, [FromQuery] string? email, [FromQuery] string? username, [FromQuery] string? externalProvider, [FromQuery] string? roles)
    {
        return sender.Send(new SearchUsersWithPaginationQuery { Email = email, Username = username, Provider = externalProvider, Roles = roles });
    }

    /// <summary>
    /// Searches for users by email based on the provided request parameters.
    /// </summary>
    /// <param name="sender">The sender used to send the search users by email request.</param>
    /// <param name="request">The request containing the email to search for.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the list of users matching the email.</returns>
    public Task<IEnumerable<UserListDTO>?> SearchUsersByEmail(ISender sender, [AsParameters] GetAccountsByEmailQuery request)
    {
        return sender.Send(request);
    }

    /// <summary>
    /// Searches for users by login provider based on the provided request parameters.
    /// </summary>
    /// <param name="sender">The sender used to send the search users by login provider request.</param>
    /// <param name="request">The request containing the login provider details to search for.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the list of users matching the login provider.</returns>
    public Task<IEnumerable<UserListDTO>?> SearchUsersByLoginProvider(ISender sender, [AsParameters] GetAccountByExternalProviderQuery request)
    {
        return sender.Send(request);
    }

    /// <summary>
    /// Searches for users by username based on the provided request parameters.
    /// </summary>
    /// <param name="sender">The sender used to send the search users by username request.</param>
    /// <param name="request">The request containing the username to search for.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the list of users matching the username.</returns>
    public Task<IEnumerable<UserListDTO>?> SearchUsersByUserName(ISender sender, [AsParameters] GetAccountByUsernameQuery request)
    {
        return sender.Send(request);
    }

    /// <summary>
    /// Creates a new user account using the provided create user command parameters.
    /// </summary>
    /// <param name="sender">The sender used to send the create user command.</param>
    /// <param name="command">The create user command containing the user's information.</param>
    /// <returns>A task that represents the asynchronous create account operation. The task result contains the result of the creation.</returns>
    public Task<Result> CreateUser(ISender sender, [FromBody] CreateUserCommand command)
    {
        return sender.Send(command);
    }

    /// <summary>
    /// Deletes a user account based on the provided user ID.
    /// </summary>
    /// <param name="sender">The sender used to send the delete account command.</param>
    /// <param name="id">The ID of the user to delete.</param>
    /// <returns>A task that represents the asynchronous delete operation. The task result contains the result of the deletion.</returns>
    public Task<Result> DeleteAccount(ISender sender, string id)
    {
        return sender.Send(new DeleteAccountCommand(id));
    }

    /// <summary>
    /// Recovers a user account using the provided recover account command parameters.
    /// </summary>
    /// <param name="sender">The sender used to send the recover account command.</param>
    /// <param name="command">The recover account command containing the user's recovery information.</param>
    /// <returns>A task that represents the asynchronous recover operation. The task result contains the result of the recovery.</returns>
    public Task<Result> RecoverAccount(ISender sender, [FromBody] RecoverAccountCommand command)
    {
        return sender.Send(command);
    }


    /// <summary>
    /// Gets a paginated list of coaches based on the provided pagination request parameters.
    /// </summary>
    /// <param name="sender">The sender used to send the get coaches list request.</param>
    /// <param name="query">The request containing pagination parameters for fetching the coaches list.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the paginated list of coaches.</returns>
    public Task<PaginatedList<CoachSummaryDTO>> GetCoachesList(ISender sender, [AsParameters] GetCoachesListWithPaginationQuery query)
    {
        return sender.Send(query);
    }

    public async Task<Result> ResetPassword(ISender sender, [FromBody] ResetPasswordCommand command)
    {
        return await sender.Send(command);
    }

    public async Task<Result> ConfirmEmail(ISender sender, [FromBody] ConfirmEmailCommand command)
    {
        return await sender.Send(command);
    }

    public async Task<Result> AuthenticatedResetPassword(ISender sender, [FromBody] AuthenticatedResetPasswordCommandDto command)
    {
        AuthenticatedResetPasswordCommand resetCommand = new AuthenticatedResetPasswordCommand
        {
            UserId = _identityService.Id ?? "",
            OldPassword = command.OldPassword,
            NewPassword = command.NewPassword
        };
        return await sender.Send(resetCommand);
    }

    /// <summary>
    /// Searches for users by username based on the provided request parameters.
    /// </summary>
    /// <param name="sender">The sender used to send the search users by username request.</param>
    /// <param name="request">The request containing the username to search for.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the list of users matching the username.</returns>
    public Task<IEnumerable<UserListDTO>> GetUsersByRole(ISender sender, [AsParameters] GetAccountsByRoleQuery request)
    {
        return sender.Send(request);
    }

    public Task<Result> UpdateUserProfile(ISender sender, [FromBody] UpdateUserCommand command)
    {

        return sender.Send(command);
    }
}
