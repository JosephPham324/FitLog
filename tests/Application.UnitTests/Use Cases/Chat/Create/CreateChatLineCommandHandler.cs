using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitLog.Application.Chats.Commands.CreateChatLine;
using FitLog.Application.Common.Interfaces;
using FitLog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace FitLog.Application.UnitTests.Use_Cases.Chat.Create;
public class CreateChatLineCommandHandlerTests
{
    private readonly Mock<IApplicationDbContext> _contextMock;
    private readonly CreateChatLineCommandHandler _handler;

    public CreateChatLineCommandHandlerTests()
    {
        _contextMock = new Mock<IApplicationDbContext>();
        _contextMock.Setup(c => c.ChatLines).Returns(Mock.Of<DbSet<ChatLine>>());
        _handler = new CreateChatLineCommandHandler(_contextMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldAddChatLineToContext()
    {
        // Arrange
        var command = new CreateChatLineCommand
        {
            ChatId = 1,
            ChatLineText = "Test Chat Line",
            LinkUrl = "http://example.com",
            AttachmentPath = "/path/to/attachment",
        };

        var chatLineId = 1;

        _contextMock.Setup(m => m.ChatLines.Add(It.IsAny<ChatLine>())).Callback<ChatLine>(chatLine => chatLine.ChatLineId = chatLineId);
        _contextMock.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        _contextMock.Verify(m => m.ChatLines.Add(It.Is<ChatLine>(c => c.ChatId == command.ChatId && c.ChatLineText == command.ChatLineText && c.LinkUrl == command.LinkUrl && c.AttachmentPath == command.AttachmentPath)), Times.Once);
        _contextMock.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        Assert.True(result.Success);
    }

    [Fact]
    public async Task Handle_ShouldReturnChatLineId()
    {
        // Arrange
        var command = new CreateChatLineCommand
        {
            ChatId = 1,
            ChatLineText = "Test Chat Line",
            LinkUrl = "http://example.com",
            AttachmentPath = "/path/to/attachment",
        };

        var chatLineId = 1;

        _contextMock.Setup(m => m.ChatLines.Add(It.IsAny<ChatLine>())).Callback<ChatLine>(chatLine => chatLine.ChatLineId = chatLineId);
        _contextMock.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.Success);
    }

    [Fact]
    public async Task Handle_ShouldThrowExceptionWhenExceptionIsThrown()
    {
        // Arrange
        var command = new CreateChatLineCommand
        {
            ChatId = 1,
            ChatLineText = "Test Chat Line",
            LinkUrl = "http://example.com",
            AttachmentPath = "/path/to/attachment",
        };

        _contextMock.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Equal("Database error", exception.Message);
    }
}

