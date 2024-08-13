//using FitLog.Application.Chats.Commands.CreateChat;
//using FitLog.Application.Common.Interfaces;
//using Moq;
//using Xunit;
//using FitLog.Domain.Entities;
//using Microsoft.EntityFrameworkCore;

//public class CreateChatCommandHandlerTests
//{
//    private readonly Mock<IApplicationDbContext> _contextMock;
//    private readonly CreateChatCommandHandler _handler;

//    public CreateChatCommandHandlerTests()
//    {
//        _contextMock = new Mock<IApplicationDbContext>();
//        _contextMock.Setup(c => c.Chats).Returns(Mock.Of<DbSet<Chat>>());
//        _handler = new CreateChatCommandHandler(_contextMock.Object);
//    }

//    [Fact]
//    public async Task Handle_ShouldAddChatToContext()
//    {
//        // Arrange
//        var command = new CreateChatCommand();
//        var chatId = 1;

//        _contextMock.Setup(m => m.Chats.Add(It.IsAny<Chat>())).Callback<Chat>(chat => chat.ChatId = chatId);
//        _contextMock.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

//        // Act
//        var result = await _handler.Handle(command, CancellationToken.None);

//        // Assert
//        _contextMock.Verify(m => m.Chats.Add(It.Is<Chat>(c => c.CreatedAt <= DateTime.UtcNow)), Times.Once);
//        _contextMock.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
//        Assert.True(result.Success);
//    }


//    [Fact]
//    public async Task Handle_ShouldReturnChatId()
//    {
//        // Arrange
//        var command = new CreateChatCommand();
//        var chatId = 1;

//        _contextMock.Setup(m => m.Chats.Add(It.IsAny<Chat>())).Callback<Chat>(chat => chat.ChatId = chatId);
//        _contextMock.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

//        // Act
//        var result = await _handler.Handle(command, CancellationToken.None);

//        // Assert
//        Assert.True(result.Success);
//    }


//    [Fact]
//    public async Task Handle_ShouldThrowExceptionWhenExceptionIsThrown()
//    {
//        // Arrange
//        var command = new CreateChatCommand();

//        _contextMock.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()))
//            .ThrowsAsync(new Exception("Database error"));

//        // Act & Assert
//        var exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
//        Assert.Equal("Database error", exception.Message);
//    }

//}

