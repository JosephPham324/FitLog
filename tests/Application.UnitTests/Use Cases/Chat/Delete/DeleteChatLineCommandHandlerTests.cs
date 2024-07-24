using System.Threading;
using System.Threading.Tasks;
using FitLog.Application.Chats.Commands.DeleteChatLine;
using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Models;
using FitLog.Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

public class DeleteChatLineCommandHandlerTests
{
    private readonly Mock<IApplicationDbContext> _contextMock;
    private readonly DeleteChatLineCommandHandler _handler;

    public DeleteChatLineCommandHandlerTests()
    {
        _contextMock = new Mock<IApplicationDbContext>();
        _handler = new DeleteChatLineCommandHandler(_contextMock.Object);
    }

    [Fact]
    public async Task Handle_GivenValidId_ShouldDeleteChatLine()
    {
        // Arrange
        var chatLineId = 1;
        var chatLine = new ChatLine { ChatLineId = 1, ChatId = 2, ChatLineText = "Test message" };

        var mockSet = new Mock<DbSet<ChatLine>>();
        mockSet.Setup(m => m.Find(It.IsAny<int>())).Returns(chatLine);

        _contextMock.Setup(m => m.ChatLines).Returns(mockSet.Object);
        _contextMock.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var command = new DeleteChatLineCommand { Id = chatLineId };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        mockSet.Verify(m => m.Remove(It.IsAny<ChatLine>()), Times.Once);
        _contextMock.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_GivenInvalidId_ShouldReturnFailure()
    { 
        // Arrange
        var chatLineId = 1;
        var mockSet = new Mock<DbSet<ChatLine>>();
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        mockSet.Setup(m => m.Find(It.IsAny<int>())).Returns<ChatLine?>(null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

        _contextMock.Setup(m => m.ChatLines).Returns(mockSet.Object);

        var command = new DeleteChatLineCommand { Id = chatLineId };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeFalse();
        result.Errors.Should().Contain("Chat line not found.");
        mockSet.Verify(m => m.Remove(It.IsAny<ChatLine>()), Times.Never);
        _contextMock.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
