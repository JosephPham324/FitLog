using FitLog.Application.Users.Commands.Regiser;
using FitLog.Application.Users.Queries.ExternalLogin;
using FitLog.Application.Users.Queries.Login;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using FitLog.Application.Users.Queries.ExternalLoginCallback;
using System;
using MediatR;
using FitLog.Application.Common.Interfaces;
using FitLog.Web.Services;

namespace FitLog.Web.Endpoints.Service_User;

public class Authentication : EndpointGroupBase
{
    private readonly IUserTokenService _tokenService;
    private readonly IUser _identityService;

    public Authentication()
    {
        _tokenService = new CurrentUserFromToken(httpContextAccessor: new HttpContextAccessor());
        _identityService = new CurrentUser(httpContextAccessor: new HttpContextAccessor());
    }
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapPost(PasswordLogin, "password-login")
            .MapPost(SignInWithGoogle, "google-login")
            .MapPost(SignInWithFacebook, "facebook-login");
            
    }

    public Task<LoginResultDTO> PasswordLogin(ISender sender, [FromBody] LoginQuery query)
    {
        return sender.Send(query);
    }

    //private async Task<string> ExternalLogin(ISender sender, string provider, string returnUrl = "/")
    //{
    //    var command = new ExternalLoginQuery { Provider = provider, ReturnUrl = returnUrl };
    //    var result = await sender.Send(command);
    //    return result;
    //}

    public async Task<LoginResultDTO> SignInWithGoogle(ISender sender, [FromBody] GoogleLoginRequest request)
    {
        var extRequest = new ExternalLoginQuery { Provider = "Google", ReturnUrl = "/", GoogleLoginRequest = request};
        
        var result = await sender.Send(extRequest);
        return result;
    }

    public async Task<LoginResultDTO> SignInWithFacebook(ISender sender, [FromBody] FacebookLoginRequest request)
    {
        var command = new ExternalLoginQuery { Provider = "Facebook", ReturnUrl = "/", FacebookLoginRequest = request };
        var result = await sender.Send(command);
        return result;
    }
}
