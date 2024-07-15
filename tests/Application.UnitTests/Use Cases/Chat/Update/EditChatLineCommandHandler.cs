using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitLog.Application.Chats.Commands.EditChatLine;
using FitLog.Application.Common.Interfaces;
using Moq;
using Xunit;

namespace FitLog.Application.UnitTests.Use_Cases.Chat.Update;
public class EditChatLineCommandHandlerTests
{
    private readonly Mock<IApplicationDbContext> _contextMock;
    private readonly EditChatLineCommandHandler _handler;

    public EditChatLineCommandHandlerTests()
    {
        _contextMock = new Mock<IApplicationDbContext>();
        _handler = new EditChatLineCommandHandler(_contextMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotImplementedException()
    {
        // Arrange
        var command = new EditChatLineCommand();

        // Act & Assert
        await Assert.ThrowsAsync<NotImplementedException>(() => _handler.Handle(command, CancellationToken.None));
    }
}

