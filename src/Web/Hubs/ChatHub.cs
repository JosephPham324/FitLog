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

    private string? GetToken()
    {
        var httpContext = Context.GetHttpContext();
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

    public async Task SendMessage(int chatId, string message)
    {

        var userId = GetUserId();

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

        await Clients.All.SendAsync("ReceiveMessage", "Me", message);
    }

    private string? GetUserId()
    {
        return _tokenService.GetUserIdFromGivenToken(GetToken() ?? "");
    }

    public async Task GetChatLines(int chatId)
    {
        var query = new GetChatLinesFromAChatQuery { ChatId = chatId };
        List<ChatLineDto> chatLines = new List<ChatLineDto>();
        foreach(var line in chatLines)
        {
            var id = line.CreatedByNavigation.Id ??"";
            if (id.Equals(GetUserId()??""))
            {
                line.CreatedByNavigation.UserName = "Me";
            }
        }
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
