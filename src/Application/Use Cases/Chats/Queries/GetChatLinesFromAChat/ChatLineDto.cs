using FitLog.Application.TodoLists.Queries.GetTodos;
using FitLog.Application.Users.Queries.GetUsers;
using FitLog.Domain.Entities;

namespace FitLog.Application.Chats.Queries.GetChatLinesFromAChat;

public class ChatLineDto
{
    public int ChatLineId { get; set; }
    public string ChatLineText { get; set; } = "";
    public string LinkUrl { get; set; } = "";
    public string AttachmentPath { get; set; } = "";
    public DateTime CreatedAt { get; set; }
    public AspNetUserListDTO CreatedByNavigation { get; set; } = new AspNetUserListDTO();

    //Mapping
    private class Mapping : AutoMapper.Profile
    {
        public Mapping()
        {
            CreateMap<ChatLine, ChatLineDto>()
                ;
        }
    }
}
