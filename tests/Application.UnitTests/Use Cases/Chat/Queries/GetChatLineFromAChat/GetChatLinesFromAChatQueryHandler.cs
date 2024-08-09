//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using AutoMapper;
//using FitLog.Application.Chats.Queries.GetChatLinesFromAChat;
//using FitLog.Application.Common.Interfaces;
//using FitLog.Domain.Entities;
//using Microsoft.EntityFrameworkCore;
//using Moq;
//using Moq.EntityFrameworkCore;
//using Xunit;

//namespace FitLog.Application.UnitTests.Use_Cases.Chat.Queries.GetChatLineFromAChat;
//public class GetChatLinesFromAChatQueryHandlerTests
//{
//    private readonly Mock<IApplicationDbContext> _contextMock;
//    private readonly GetChatLinesFromAChatQueryHandler _handler;

//    public GetChatLinesFromAChatQueryHandlerTests()
//    {
//        _contextMock = new Mock<IApplicationDbContext>();
//        var config = new MapperConfiguration(cfg =>
//                {
//                });
//        _handler = new GetChatLinesFromAChatQueryHandler(_contextMock.Object, config.CreateMapper());
//    }

//    [Fact]
//    public async Task Handle_ShouldReturnListOfChatLineDtos()
//    {
//        // Arrange
//        var chatId = 1;
//        var chatLines = new List<ChatLine>
//            {
//                new ChatLine { ChatLineId = 1, ChatId = chatId, ChatLineText = "Text1", LinkUrl = "Url1", AttachmentPath = "Path1", CreatedAt = DateTime.UtcNow },
//                new ChatLine { ChatLineId = 2, ChatId = chatId, ChatLineText = "Text2", LinkUrl = "Url2", AttachmentPath = "Path2", CreatedAt = DateTime.UtcNow }
//            };

//        _contextMock.Setup(c => c.ChatLines).ReturnsDbSet(chatLines);

//        var query = new GetChatLinesFromAChatQuery { ChatId = chatId };

//        // Act
//        var result = await _handler.Handle(query, CancellationToken.None);

//        // Assert
//        Assert.NotNull(result);
//        Assert.Equal(2, result.Count);
//        Assert.Equal(chatLines[0].ChatLineId, result[0].ChatLineId);
//        Assert.Equal(chatLines[0].ChatLineText, result[0].ChatLineText);
//        Assert.Equal(chatLines[0].LinkUrl, result[0].LinkUrl);
//        Assert.Equal(chatLines[0].AttachmentPath, result[0].AttachmentPath);
//        Assert.Equal(chatLines[0].CreatedAt, result[0].CreatedAt);

//        Assert.Equal(chatLines[1].ChatLineId, result[1].ChatLineId);
//        Assert.Equal(chatLines[1].ChatLineText, result[1].ChatLineText);
//        Assert.Equal(chatLines[1].LinkUrl, result[1].LinkUrl);
//        Assert.Equal(chatLines[1].AttachmentPath, result[1].AttachmentPath);
//        Assert.Equal(chatLines[1].CreatedAt, result[1].CreatedAt);
//    }

//    [Fact]
//    public async Task Handle_ShouldReturnEmptyListWhenNoChatLinesFound()
//    {
//        // Arrange
//        var chatId = 1;

//        var chatLines = new List<ChatLine>();

//        _contextMock.Setup(c => c.ChatLines).ReturnsDbSet(chatLines);

//        var query = new GetChatLinesFromAChatQuery { ChatId = chatId };

//        // Act
//        var result = await _handler.Handle(query, CancellationToken.None);

//        // Assert
//        Assert.NotNull(result);
//        Assert.Empty(result);
//    }
//}


