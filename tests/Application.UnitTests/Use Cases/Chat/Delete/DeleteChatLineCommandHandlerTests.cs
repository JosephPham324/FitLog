using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitLog.Application.Chats.Commands.DeleteChatLine;
using FitLog.Application.Common.Interfaces;
using Moq;
using Xunit;

namespace FitLog.Application.UnitTests.Use_Cases.Chat.Delete;
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
    public async Task Handle_ShouldThrowNotImplementedException()
    {
        // Arrange
        var command = new DeleteChatLineCommand();

        // Act & Assert
        await Assert.ThrowsAsync<NotImplementedException>(() => _handler.Handle(command, CancellationToken.None));
    }
}

