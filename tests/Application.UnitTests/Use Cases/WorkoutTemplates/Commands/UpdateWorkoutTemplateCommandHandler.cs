using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using FitLog.Application.Common.Exceptions;
using FitLog.Application.Common.Interfaces;
using FitLog.Application.WorkoutTemplates.Commands.UpdateWorkoutTemplate;
using FitLog.Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using FitLog.Application.Common.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using System;
using MockQueryable.Moq;

namespace FitLog.Application.WorkoutTemplates.Commands.UpdateWorkoutTemplate;

public class UpdateWorkoutTemplateCommandHandlerTests
{
    private readonly Mock<IApplicationDbContext> _mockContext;
    private readonly UpdateWorkoutTemplateCommandHandler _handler;

    public UpdateWorkoutTemplateCommandHandlerTests()
    {
        _mockContext = new Mock<IApplicationDbContext>();
        _handler = new UpdateWorkoutTemplateCommandHandler(_mockContext.Object);
    }

    [Fact]
    public async Task Handle_GivenValidRequest_UpdatesWorkoutTemplate()
    {
        // Arrange
        var command = new UpdateWorkoutTemplateCommand
        {
            Id = 1,
            TemplateName = "Updated Template",
            Duration = "01:30",
            IsPublic = true,
            WorkoutTemplateExercises = new List<WorkoutTemplateExerciseDto>
                {
                    new WorkoutTemplateExerciseDto
                    {
                        ExerciseId = 1,
                        OrderInSession = 1,
                        Note = "Updated note"
                    }
                }
        };

        var workoutTemplate = new WorkoutTemplate
        {
            Id = 1,
            TemplateName = "Original Template",
            Duration = "01:00",
            IsPublic = false,
            WorkoutTemplateExercises = new List<WorkoutTemplateExercise>
                {
                    new WorkoutTemplateExercise
                    {
                        ExerciseId = 1,
                        OrderInSession = 1,
                        Note = "Original note"
                    }
                }
        };

        var workoutTemplates = new List<WorkoutTemplate> { workoutTemplate }.AsQueryable().BuildMockDbSet();

        _mockContext.Setup(x => x.WorkoutTemplates).Returns(workoutTemplates.Object);
        _mockContext.Setup(x => x.WorkoutTemplateExercises).Returns(workoutTemplate.WorkoutTemplateExercises.AsQueryable().BuildMockDbSet().Object);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        _mockContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        workoutTemplate.TemplateName.Should().Be("Updated Template");
        workoutTemplate.Duration.Should().Be("01:30");
        workoutTemplate.IsPublic.Should().BeTrue();
        workoutTemplate.WorkoutTemplateExercises.First().Note.Should().Be("Updated note");
    }

    [Fact]
    public async Task Handle_GivenNonExistentTemplate_ReturnsFailureResult()
    {
        // Arrange
        var command = new UpdateWorkoutTemplateCommand
        {
            Id = 2, // Non-existent ID
            TemplateName = "Updated Template",
            Duration = "01:30",
            IsPublic = true,
            WorkoutTemplateExercises = new List<WorkoutTemplateExerciseDto>
                {
                    new WorkoutTemplateExerciseDto
                    {
                        ExerciseId = 1,
                        OrderInSession = 1,
                        Note = "Updated note"
                    }
                }
        };

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
    public async Task Handle_GivenInvalidTemplateName_ValidationReturnsFalse()
    {
        // Arrange
        var command = new UpdateWorkoutTemplateCommand
        {
            Id = 1,
            TemplateName = "", // Invalid template name
            Duration = "01:30",
            IsPublic = true,
            WorkoutTemplateExercises = new List<WorkoutTemplateExerciseDto>
                {
                    new WorkoutTemplateExerciseDto
                    {
                        ExerciseId = 1,
                        OrderInSession = 1,
                        Note = "Updated note"
                    }
                }
        };

        var validator = new UpdateWorkoutTemplateCommandValidator();
        var validationResult = await validator.ValidateAsync(command);

        // Assert
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Should().ContainSingle(e => e.PropertyName == "TemplateName");
    }

    [Fact]
    public async Task Handle_GivenInvalidExerciseId_ValidationReturnsFalse()
    {
        // Arrange
        var command = new UpdateWorkoutTemplateCommand
        {
            Id = 1,
            TemplateName = "Updated Template",
            Duration = "01:30",
            IsPublic = true,
            WorkoutTemplateExercises = new List<WorkoutTemplateExerciseDto>
                {
                    new WorkoutTemplateExerciseDto
                    {
                        ExerciseId = null, // Invalid exercise ID
                        OrderInSession = 1,
                        Note = "Updated note"
                    }
                }
        };

        var validator = new UpdateWorkoutTemplateCommandValidator();
        var validationResult = await validator.ValidateAsync(command);

        // Assert
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Should().ContainSingle(e => e.PropertyName == "WorkoutTemplateExercises[0].ExerciseId");
    }
}
