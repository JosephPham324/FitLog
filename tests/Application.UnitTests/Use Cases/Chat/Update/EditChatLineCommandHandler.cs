using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FitLog.Application.Chats.Commands.EditChatLine;
using FitLog.Application.Common.Interfaces;
using Moq;
using Xunit;

namespace FitLog.Application.UnitTests.Use_Cases.Chat.Update;
public class EditChatLineCommandHandlerTests
{
    private readonly Mock<IApplicationDbContext> _contextMock;
    private readonly EditChatLineCommandHandler _handler;
    private readonly IMapper _mapper;

    public EditChatLineCommandHandlerTests()
    {
        _contextMock = new Mock<IApplicationDbContext>();
        var config = new MapperConfiguration(cfg =>
                 {
                 });
        _mapper = config.CreateMapper();
        _handler = new EditChatLineCommandHandler(_contextMock.Object, _mapper);
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

