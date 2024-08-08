using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitLog.Application.Common.Interfaces;
using FitLog.Application.WorkoutTemplates.Commands.CreateWorkoutTemplate;
using FitLog.Domain.Entities;
using FluentAssertions;
using MockQueryable.Moq;
using Moq;
using Xunit;

namespace FitLog.Application.UnitTests.Use_Cases.WorkoutTemplates.Create;
public class CreateWorkoutTemplateCommandHandlerTests
{
    private readonly Mock<IApplicationDbContext> _mockContext;
    private readonly CreateWorkoutTemplateCommandHandler _handler;

    public CreateWorkoutTemplateCommandHandlerTests()
    {
        _mockContext = new Mock<IApplicationDbContext>();
        _handler = new CreateWorkoutTemplateCommandHandler(_mockContext.Object);
    }

    [Fact]
    public async Task Handle_GivenValidRequest_CreatesWorkoutTemplate()
    {
        // Arrange
        var command = new CreateWorkoutTemplateCommand
        {
            UserId = "user123",
            TemplateName = "My Template",
            Duration = "01:00",
            IsPublic = true,
            WorkoutTemplateExercises = new List<WorkoutTemplateExerciseDto>
                {
                    new WorkoutTemplateExerciseDto
                    {
                        ExerciseId = 1,
                        OrderInSession = 1,
                        Note = "Some note"
                    }
                }
        };

        var workoutTemplates = new List<WorkoutTemplate>().AsQueryable().BuildMockDbSet();

        _mockContext.Setup(x => x.WorkoutTemplates).Returns(workoutTemplates.Object);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        _mockContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_GivenInvalidTemplateName_ValidationReturnsFalse()
    {
        // Arrange
        var command = new CreateWorkoutTemplateCommand
        {
            UserId = "user123",
            TemplateName = "", // Invalid template name
            Duration = "01:00",
            IsPublic = true,
            WorkoutTemplateExercises = new List<WorkoutTemplateExerciseDto>
                {
                    new WorkoutTemplateExerciseDto
                    {
                        ExerciseId = 1,
                        OrderInSession = 1,
                        Note = "Some note"
                    }
                }
        };

        var validator = new CreateWorkoutTemplateCommandValidator();
        var validationResult = await validator.ValidateAsync(command);

        // Assert
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Should().ContainSingle(e => e.PropertyName == "TemplateName");
    }

    [Fact]
    public async Task Handle_GivenInvalidExerciseId_ValidationReturnsFalse()
    {
        // Arrange
        var command = new CreateWorkoutTemplateCommand
        {
            UserId = "user123",
            TemplateName = "My Template",
            Duration = "01:00",
            IsPublic = true,
            WorkoutTemplateExercises = new List<WorkoutTemplateExerciseDto>
                {
                    new WorkoutTemplateExerciseDto
                    {
                        ExerciseId = null, // Invalid exercise ID
                        OrderInSession = 1,
                        Note = "Some note"
                    }
                }
        };

        var validator = new CreateWorkoutTemplateCommandValidator();
        var validationResult = await validator.ValidateAsync(command);

        // Assert
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Should().ContainSingle(e => e.PropertyName == "WorkoutTemplateExercises[0].ExerciseId");
    }
}
