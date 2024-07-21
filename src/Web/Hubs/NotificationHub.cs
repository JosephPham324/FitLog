using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using FitLog.Application.Common.Interfaces;
using Microsoft.AspNetCore.SignalR;
using YamlDotNet.Core.Tokens;

namespace FitLog.Web.Hubs;

public class NotificationHub : Hub
{

    private static readonly ConcurrentDictionary<string, string> _connections = new ConcurrentDictionary<string, string>();
    //private static readonly IUserTokenService _userTokenService = new Services.CurrentUserFromToken();
    //public NotificationHub(IUserTokenService tokenService)
    //{
    //    _userTokenService = tokenService;
    //}
    private static string? GetToken(HttpContext? httpContext)
    {
        if (httpContext == null)
        {
            return null;
        }
        // Try to get the token from the Authorization header
        var authorizationHeader = httpContext?.Request.Headers["Authorization"].ToString();
        if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
        {
            return authorizationHeader.Substring("Bearer ".Length).Trim();
        }

        // If not found in the header, try to get it from the query string
        var token = httpContext?.Request.Query["access_token"].ToString();
        if (!string.IsNullOrEmpty(token))
        {
            return token;
        }

        return null;
    }
    private string? GetUserId(string? token)
    {
        if (string.IsNullOrEmpty(token))
        {
            return null;
        }

        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

        var userId = jsonToken?.Claims.FirstOrDefault(claim => claim.Type == "Id")?.Value;

        return userId;
    }


    public override Task OnConnectedAsync()
    {
        // Assuming you have a way to identify the user, e.g., through a query string or authentication
        var token = GetToken(Context.GetHttpContext());
        var id = GetUserId(token);
        if (id == null)
        {
            return base.OnConnectedAsync();
        }
        _connections[id] = Context.ConnectionId;
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        var token = GetToken(Context.GetHttpContext());
        var id = GetUserId(token);
        if (id == null)
        {
            return base.OnDisconnectedAsync(exception);
        }
        _connections.TryRemove(id, out _);
        return base.OnDisconnectedAsync(exception);
    }

    public static string? GetConnectionId(string userId)
    {
        _connections.TryGetValue(userId, out var connectionId);
        return connectionId;
    }
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }
}
