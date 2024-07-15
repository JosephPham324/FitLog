using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using FitLog.Application.Common.Interfaces;
using FitLog.Application.WorkoutTemplates.Commands.DeleteWorkoutTemplate;
using FitLog.Domain.Entities;
using Moq;
using Xunit;

namespace FitLog.Application.UnitTests.Use_Cases.WorkoutTemplates.Delete;
public class DeleteWorkoutTemplateCommandHandlerTests
{
    private readonly Mock<IApplicationDbContext> _contextMock;
    private readonly DeleteWorkoutTemplateCommandHandler _handler;

    public DeleteWorkoutTemplateCommandHandlerTests()
    {
        _contextMock = new Mock<IApplicationDbContext>();
        _handler = new DeleteWorkoutTemplateCommandHandler(_contextMock.Object);
    }

    [Fact]
    public async Task Handle_ValidId_ShouldDeleteWorkoutTemplate()
    {
        // Arrange
        var command = new DeleteWorkoutTemplateCommand(1);
        var entity = new WorkoutTemplate { Id = command.Id };

        _contextMock.Setup(m => m.WorkoutTemplates.FindAsync(new object[] { command.Id }, CancellationToken.None))
                    .ReturnsAsync(entity);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        _contextMock.Verify(m => m.WorkoutTemplates.FindAsync(new object[] { command.Id }, CancellationToken.None), Times.Once);
        _contextMock.Verify(m => m.WorkoutTemplates.Remove(entity), Times.Once);
        _contextMock.Verify(m => m.SaveChangesAsync(CancellationToken.None), Times.Once);
        Assert.True(result);
    }

    [Fact]
    public async Task Handle_InvalidId_ShouldThrowNotFoundException()
    {
        // Arrange
        var command = new DeleteWorkoutTemplateCommand(999); // Non-existent ID
        WorkoutTemplate? nullEntity = null;

        _contextMock.Setup(m => m.WorkoutTemplates.FindAsync(new object[] { command.Id }, CancellationToken.None))
                    .ReturnsAsync(nullEntity);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));

        _contextMock.Verify(m => m.WorkoutTemplates.FindAsync(new object[] { command.Id }, CancellationToken.None), Times.Once);
        _contextMock.Verify(m => m.WorkoutTemplates.Remove(It.IsAny<WorkoutTemplate>()), Times.Never);
        _contextMock.Verify(m => m.SaveChangesAsync(CancellationToken.None), Times.Never);
    }
}

