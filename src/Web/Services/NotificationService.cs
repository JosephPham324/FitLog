using FitLog.Application.Common.Interfaces;
using FitLog.Web.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace FitLog.Web.Services;
public class SignalRNotificationService : INotificationService
{
    private readonly IHubContext<NotificationHub> _hubContext;

    public SignalRNotificationService(IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task SendNotificationAsync(string user, string message)
    {
        await _hubContext.Clients.All.SendAsync("ReceiveMessage", user, message);
    }
}
