namespace FitLog.Application.Chats.Queries.GetChatLinesFromAChat;

public class ChatLineDto
{
    public int ChatLineId { get; set; }
    public string ChatLineText { get; set; } = "";
    public string LinkUrl { get; set; } = "";
    public string AttachmentPath { get; set; } = "";
    public DateTime CreatedAt { get; set; }
}
