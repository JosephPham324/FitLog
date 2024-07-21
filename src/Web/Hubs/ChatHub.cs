using System.Collections.Generic;
using FitLog.Application.Chats.Commands.CreateChatLine;
using FitLog.Application.Chats.Commands.DeleteChatLine;
using FitLog.Application.Chats.Commands.EditChatLine;
using FitLog.Application.Chats.Queries.GetChatLinesFromAChat;
using FitLog.Application.Chats.Queries.GetChatMedia;
using FitLog.Application.Chats.Queries.GetChatUrls;
using FitLog.Application.Common.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System.IdentityModel.Tokens.Jwt;

namespace FitLog.Web.Hubs;

public class ChatHub : Hub
{
    private readonly ISender _sender;
    private readonly IUserTokenService _tokenService;
    #region hub configs
    public ChatHub(ISender sender, IUserTokenService tokenService)
    {
        _sender = sender;
        _tokenService = tokenService;
    }

    public override async Task OnConnectedAsync()
    {
        var httpContext = Context.GetHttpContext();
        var userId = GetUserId(httpContext);
        if (userId != null)
        {
            // Add the connection ID to a user-specific group
            await Groups.AddToGroupAsync(Context.ConnectionId, userId);
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var httpContext = Context.GetHttpContext();
        var userId = GetUserId(httpContext);
        if (userId != null)
        {
            // Remove the connection ID from the user-specific group
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId);
        }

        await base.OnDisconnectedAsync(exception);
    }
    #endregion

    #region Token service

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

    private string? GetUserId(HttpContext? httpContext)
    {

        var token = GetToken(httpContext);
        return _tokenService.GetUserIdFromGivenToken(token ?? "");
    }
    #endregion

    // Add the client to the chat group
    public async Task JoinChatGroup(int chatId)
    {
        var userId = GetUserId(Context.GetHttpContext());

        if (userId == null)
        {
            // Return unauthorized
            throw new UnauthorizedAccessException("User is not authenticated yet");
        }

        await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
    }

    // Remove the client from the chat group
    public async Task LeaveChatGroup(int chatId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId.ToString());
    }

    public async Task SendMessage(int chatId, string message)
    {
        var userId = GetUserId(Context.GetHttpContext());

        if (userId == null)
        {
            // Return unauthorized
            throw new UnauthorizedAccessException("User is not authenticated yet");
        }
        var command = new CreateChatLineCommand
        {
            ChatId = chatId,
            UserId = userId,
            ChatLineText = message
        };

        var result = await _sender.Send(command);

        await Clients.Group(chatId.ToString()).SendAsync("ReceiveMessage", result.ChatLine.ChatLineId, "Me", message);
    }

    public async Task GetChatLines(int chatId)
    {
        var query = new GetChatLinesFromAChatQuery { ChatId = chatId };
        List<ChatLineDto> chatLines = new List<ChatLineDto>();
        try
        {
            chatLines = await _sender.Send(query);
            foreach (var line in chatLines)
            {
                var id = line.CreatedByNavigation.Id ?? "";
                if (id.Equals(GetUserId(Context.GetHttpContext()) ?? ""))
                {
                    line.CreatedByNavigation.UserName = "Me";
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        await Clients.Group(chatId.ToString()).SendAsync("LoadMessages", chatLines);
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

    public async Task UpdateChatLine(int chatLineId, string message)
    {
        var userId = GetUserId(Context.GetHttpContext());

        if (userId == null)
        {
            // Return unauthorized
            throw new UnauthorizedAccessException("User is not authenticated yet");
        }
        var command = new EditChatLineCommand
        {
            Id = chatLineId,
            ChatLineText = message,
            AttachmentPath = "",
            LinkUrl = "",
        };

        var updateResult = await _sender.Send(command);

        await Clients.Group(command.Id.ToString()).SendAsync("UpdatedMessage", updateResult);
    }

    public async Task DeleteChatLine(int chatLineId)
    {
        var command = new DeleteChatLineCommand
        {
            Id = chatLineId
        };
        var res = await _sender.Send(command);
        await Clients.Group(command.Id.ToString()).SendAsync("DeletedMessage", res);
    }
}
