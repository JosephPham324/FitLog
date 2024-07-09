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

    public async Task<string> SignInWithGoogle(ISender sender, [FromBody] GoogleLoginRequest request)
    {
        var extRequest = new ExternalLoginQuery { Provider = "Google", ReturnUrl = "/", GoogleLoginRequest = request};
        
        var jwtToken = await sender.Send(extRequest);
        return jwtToken;
    }

    public async Task<string> SignInWithFacebook(ISender sender, [FromBody] FacebookLoginRequest request)
    {
        var command = new ExternalLoginQuery { Provider = "Facebook", ReturnUrl = "/", FacebookLoginRequest = request };
        var jwtToken = await sender.Send(command);
        return jwtToken;
    }
}
