
using FitLog.Application.Chats.Commands.CreateChat;
using FitLog.Application.Chats.Queries.GetChatsOfUser;
using FitLog.Application.Common.Models;
using FitLog.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FitLog.Web.Endpoints.Service_Chat;

public class Chats : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireAuthorization()
            .MapGet(GetUserChatList, "user")
            .MapPost(CreateChat, "create");
        //throw new NotImplementedException();
    }

    public async Task<List<Chat>> GetUserChatList(ISender sender, [AsParameters] GetChatsOfUserQuery query)
    {
        var chats = await sender.Send(query);
        return chats;
    }

    public async Task<Result> CreateChat(ISender sender, [FromBody] CreateChatCommand command)
    {
        return await sender.Send(command);
    }
}
