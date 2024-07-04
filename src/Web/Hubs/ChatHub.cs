using FitLog.Application.Chats.Commands.CreateChatLine;
using FitLog.Application.Chats.Queries.GetChatLinesFromAChat;
using Microsoft.AspNetCore.SignalR;

namespace FitLog.Web.Hubs;

public class ChatHub : Hub
{
    //public async Task SendMessage(ISender sender, int chatId, string user, string message)
    //{
    //    var command = new CreateChatLineCommand
    //    {
    //        ChatId = chatId,
    //        ChatLineText = message,
    //        CreatedAt = DateTime.UtcNow
    //    };

    //    await sender.Send(command);

    //    await Clients.All.SendAsync("ReceiveMessage", user, message);
    //}

    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }

    public async Task<List<ChatLineDto>> GetChatLines(ISender sender, int chatId)
    {
        var query = new GetChatLinesFromAChatQuery { ChatId = chatId };
        return await sender.Send(query);
    }
}
