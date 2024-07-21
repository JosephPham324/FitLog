using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using FitLog.Application.Common.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace FitLog.Web.Hubs
{
    public class NotificationHub : Hub
    {
        private static readonly ConcurrentDictionary<string, string> _connections = new ConcurrentDictionary<string, string>();
        private readonly ILogger<NotificationHub> _logger;

        public NotificationHub(ILogger<NotificationHub> logger)
        {
            _logger = logger;
        }

        private static string? GetToken(HttpContext? httpContext)
        {
            if (httpContext == null)
            {
                return null;
            }

            var authorizationHeader = httpContext.Request.Headers["Authorization"].ToString();
            if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
            {
                return authorizationHeader.Substring("Bearer ".Length).Trim();
            }

            var token = httpContext.Request.Query["access_token"].ToString();
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

            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
                var userId = jsonToken?.Claims.FirstOrDefault(claim => claim.Type == "Id")?.Value;
                return userId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error parsing token to extract user ID.");
                return null;
            }
        }

        public override async Task OnConnectedAsync()
        {
            _logger.LogInformation("Client connected. Connection ID: {ConnectionId}", Context.ConnectionId);

            var token = GetToken(Context.GetHttpContext());
            var userId = GetUserId(token);
            if (userId != null)
            {
                _connections[userId] = Context.ConnectionId;
                _logger.LogInformation("User {UserId} connected with Connection ID: {ConnectionId}", userId, Context.ConnectionId);
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            _logger.LogInformation("Client disconnected. Connection ID: {ConnectionId}", Context.ConnectionId);

            var token = GetToken(Context.GetHttpContext());
            var userId = GetUserId(token);
            if (userId != null)
            {
                _connections.TryRemove(userId, out _);
                _logger.LogInformation("User {UserId} disconnected. Connection ID: {ConnectionId} removed.", userId, Context.ConnectionId);
            }

            await base.OnDisconnectedAsync(exception);
        }

        public static string? GetConnectionId(string userId)
        {
            _connections.TryGetValue(userId, out var connectionId);
            return connectionId;
        }

        public async Task SendMessage(string user, string message)
        {
            _logger.LogInformation("Sending message to user {User}: {Message}", user, message);
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
