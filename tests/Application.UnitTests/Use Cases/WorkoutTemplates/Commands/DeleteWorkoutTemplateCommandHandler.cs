using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using FitLog.Application.Common.Interfaces;
using FitLog.Application.WorkoutTemplates.Commands.DeleteWorkoutTemplate;
using FitLog.Domain.Entities;
using FluentAssertions;
using MockQueryable.Moq;
using Moq;
using Xunit;

namespace FitLog.Application.UnitTests.Use_Cases.WorkoutTemplates.Delete;
public class DeleteWorkoutTemplateCommandHandlerTests
{
    private readonly Mock<IApplicationDbContext> _mockContext;
    private readonly DeleteWorkoutTemplateCommandHandler _handler;

    public DeleteWorkoutTemplateCommandHandlerTests()
    {
        _mockContext = new Mock<IApplicationDbContext>();
        _handler = new DeleteWorkoutTemplateCommandHandler(_mockContext.Object);
    }

    [Fact]
    public async Task Handle_GivenValidRequest_DeletesWorkoutTemplate()
    {
        // Arrange
        var command = new DeleteWorkoutTemplateCommand(1);

        var workoutTemplate = new WorkoutTemplate
        {
            Id = 1,
            TemplateName = "Template1",
            WorkoutTemplateExercises = new List<WorkoutTemplateExercise>
                {
                    new WorkoutTemplateExercise { ExerciseId = 1, OrderInSession = 1 }
                }
        };

        var workoutTemplates = new List<WorkoutTemplate> { workoutTemplate }.AsQueryable().BuildMockDbSet();
        _mockContext.Setup(x => x.WorkoutTemplates).Returns(workoutTemplates.Object);

        var workoutTemplateExercises = workoutTemplate.WorkoutTemplateExercises.AsQueryable().BuildMockDbSet();
        _mockContext.Setup(x => x.WorkoutTemplateExercises).Returns(workoutTemplateExercises.Object);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        _mockContext.Verify(x => x.WorkoutTemplates.Remove(It.IsAny<WorkoutTemplate>()), Times.Once);
        _mockContext.Verify(x => x.WorkoutTemplateExercises.RemoveRange(It.IsAny<IEnumerable<WorkoutTemplateExercise>>()), Times.Once);
        _mockContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Exactly(2));
    }

    [Fact]
    public async Task Handle_GivenNonExistentTemplate_ReturnsFailureResult()
    {
        // Arrange
        var command = new DeleteWorkoutTemplateCommand(2); // Non-existent ID

        var workoutTemplates = new List<WorkoutTemplate>().AsQueryable().BuildMockDbSet();
        _mockContext.Setup(x => x.WorkoutTemplates).Returns(workoutTemplates.Object);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e == "Workout Template not found");
        _mockContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_InvalidTemplateId_ValidationReturnsFalse()
    {
        // Arrange
        var command = new DeleteWorkoutTemplateCommand(0); // Invalid ID

        var validator = new DeleteWorkoutTemplateCommandValidator();
        var validationResult = await validator.ValidateAsync(command);

        // Assert
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Should().ContainSingle(e => e.PropertyName == "Id" && e.ErrorMessage == "Invalid workout template ID.");
    }
}


