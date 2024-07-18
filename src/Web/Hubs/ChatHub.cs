using System.Collections.Generic;
using FitLog.Application.Chats.Commands.CreateChatLine;
using FitLog.Application.Chats.Queries.GetChatLinesFromAChat;
using FitLog.Application.Chats.Queries.GetChatMedia;
using FitLog.Application.Chats.Queries.GetChatUrls;
using FitLog.Application.Common.Interfaces;
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

    private readonly ISender _sender;
    private readonly IUserTokenService _tokenService;

    public ChatHub(ISender sender, IUserTokenService tokenService)
    {
        _sender = sender;
        _tokenService = tokenService;
    }

    public async Task SendMessage(int chatId, string message)
    {
        var userId = _tokenService.GetUserIdFromToken();
        if (userId == null)
        {
            //Return unauthorized
            throw new UnauthorizedAccessException("User is not authenticated yet");
        }
        var command = new CreateChatLineCommand
        {
            ChatId = chatId,
            UserId = userId ?? "",
            ChatLineText = message
        };

        await _sender.Send(command);

        await Clients.All.SendAsync("ReceiveMessage", userId, message);
    }

    public async Task GetChatLines(int chatId)
    {
        var query = new GetChatLinesFromAChatQuery { ChatId = chatId };
        List<ChatLineDto> chatLines = new List<ChatLineDto>();
        try
        {
            chatLines = await _sender.Send(query);

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        await Clients.All.SendAsync("LoadMessages", chatLines );
    }

    public async Task<List<string>> GetChatUrls(int chatId)
    {
        var query = new GetChatUrlsQuery { ChatId = chatId };
        return await _sender.Send(query);
    }

    public async Task<List<string>> GetChatMedia(int chatId)
    {
        var query = new GetChatMediaQuery { ChatId = chatId };
        return await _sender.Send(query);
    }


}
