using FitLog.Application.Users.Commands.Regiser;
using FitLog.Application.Users.Queries.ExternalLogin;
using FitLog.Application.Users.Queries.Login;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using FitLog.Application.Users.Queries.ExternalLoginCallback;
using System;
using MediatR;

namespace FitLog.Web.Endpoints.Service_User;

public class Authentication : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapPost(PasswordLogin, "password-login")
            .MapPost(SignInWithGoogle, "google-login");
    }

    public Task<LoginResultDTO> PasswordLogin(ISender sender, [AsParameters] LoginQuery query)
    {
        
        return sender.Send(query);
    }

    //private async Task<string> ExternalLogin(ISender sender, string provider, string returnUrl = "/")
    //{
    //    var command = new ExternalLoginQuery { Provider = provider, ReturnUrl = returnUrl };
    //    var result = await sender.Send(command);
    //    return result;
    //}

    public async Task<string> SignInWithGoogle(ISender sender, [FromBody] GoogleLoginRequest request)
    {
        var command = new ExternalLoginQuery { Provider = "Google", ReturnUrl = "/", Token = request.Token };
        
        var jwtToken = await sender.Send(command);
        return jwtToken;
    }

    //[HttpGet("signin-facebook")]
    //public IActionResult SignInWithFacebook(string returnUrl = "/")
    //{
    //    var properties = new AuthenticationProperties { RedirectUri = returnUrl };
    //    return Challenge(properties, FacebookDefaults.AuthenticationScheme);
    //}
}
