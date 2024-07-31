using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Models;
using FitLog.Application.WorkoutLogs.Commands.UpdateWorkoutLog;
using FitLog.Domain.Entities;
using Moq;
using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using MockQueryable.Moq;

namespace FitLog.Application.UnitTests.Use_Cases.WorkoutLogs.Commands;
public class UpdateWorkoutLogCommandHandlerTests
{
    private readonly Mock<IApplicationDbContext> _contextMock;
    private readonly UpdateWorkoutLogCommandHandler _handler;
    private readonly UpdateWorkoutLogCommandValidator _validator;

    public UpdateWorkoutLogCommandHandlerTests()
    {
        // Create mock instances
        _contextMock = new Mock<IApplicationDbContext>();

        var workoutLogsDbSetMock = new Mock<DbSet<WorkoutLog>>();
        var exerciseLogsDbSetMock = new Mock<DbSet<ExerciseLog>>();

        _contextMock.Setup(x => x.WorkoutLogs).Returns(workoutLogsDbSetMock.Object);
        _contextMock.Setup(x => x.ExerciseLogs).Returns(exerciseLogsDbSetMock.Object);

        _handler = new UpdateWorkoutLogCommandHandler(_contextMock.Object);
        _validator = new UpdateWorkoutLogCommandValidator();
    }

    [Fact]
    public async Task Handle_Should_Update_WorkoutLog_When_Command_Is_Valid()
    {
        // Arrange
        var workoutLog = new WorkoutLog
        {
            Id = 1,
            Note = "Old Note",
            Duration = new TimeOnly(1, 0),
            ExerciseLogs = new List<ExerciseLog>
                {
                    new ExerciseLog
                    {
                        ExerciseLogId = 1,
                        ExerciseId = 1,
                        Note = "Old Exercise Note",
                        NumberOfSets = 3,
                        WeightsUsed = "[100,100,100]",
                        NumberOfReps = "[10,10,10]",
                        FootageUrls = "[\"https://example.com/footage1\"]"
                    }
                }
        };

        var command = new UpdateWorkoutLogCommand
        {
            WorkoutLogId = 1,
            Note = "Updated Note",
            Duration = new TimeOnly(1, 30),
            ExerciseLogs = new List<UpdateExerciseLogCommand>
                {
                    new UpdateExerciseLogCommand
                    {
                        ExerciseLogId = 1,
                        ExerciseId = 2,
                        Note = "Updated Exercise Note",
                        NumberOfSets = 4,
                        WeightsUsedValue = new List<int> { 110, 110, 110 },
                        NumberOfRepsValue = new List<int> { 12, 12, 12 },
                        FootageUrls = "[\"https://example.com/footage2\"]"
                    }
                }
        };

        var workoutLogs = new List<WorkoutLog> { workoutLog }.AsQueryable().BuildMockDbSet();
        _contextMock.Setup(x => x.WorkoutLogs).Returns(workoutLogs.Object);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Success.Should().BeTrue();
        workoutLog.Note.Should().Be("Updated Note");
        workoutLog.Duration.Should().Be(new TimeOnly(1, 30));
        var updatedExerciseLog = workoutLog.ExerciseLogs.First();
        updatedExerciseLog.ExerciseId.Should().Be(2);
        updatedExerciseLog.Note.Should().Be("Updated Exercise Note");
        updatedExerciseLog.NumberOfSets.Should().Be(4);
        updatedExerciseLog.WeightsUsed.Should().Be("[110,110,110]");
        updatedExerciseLog.NumberOfReps.Should().Be("[12,12,12]");
        updatedExerciseLog.FootageUrls.Should().Be("[\"https://example.com/footage2\"]");

        _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Return_Failure_When_WorkoutLog_Not_Found()
    {
        // Arrange
        var command = new UpdateWorkoutLogCommand
        {
            WorkoutLogId = 99,
            Note = "Updated Note",
            Duration = new TimeOnly(1, 30),
            ExerciseLogs = new List<UpdateExerciseLogCommand>()
        };

        var workoutLogs = new List<WorkoutLog>().AsQueryable().BuildMockDbSet();
        _contextMock.Setup(x => x.WorkoutLogs).Returns(workoutLogs.Object);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Success.Should().BeFalse();
        result.Errors.Should().Contain($"Workout log with ID {command.WorkoutLogId} not found.");
    }

    [Fact]
    public async Task Handle_Should_Remove_ExerciseLog_When_IsDeleted_Is_True()
    {
        // Arrange
        var workoutLog = new WorkoutLog
        {
            Id = 1,
            Note = "Old Note",
            Duration = new TimeOnly(1, 0),
            ExerciseLogs = new List<ExerciseLog>
                {
                    new ExerciseLog
                    {
                        ExerciseLogId = 1,
                        ExerciseId = 1,
                        Note = "Old Exercise Note",
                        NumberOfSets = 3,
                        WeightsUsed = "[100, 100, 100]",
                        NumberOfReps = "[10, 10, 10]",
                        FootageUrls = "[\"https://example.com/footage1\"]"
                    }
                }
        };

        var command = new UpdateWorkoutLogCommand
        {
            WorkoutLogId = 1,
            Note = "Updated Note",
            Duration = new TimeOnly(1, 30),
            ExerciseLogs = new List<UpdateExerciseLogCommand>
                {
                    new UpdateExerciseLogCommand
                    {
                        ExerciseLogId = 1,
                        IsDeleted = true
                    }
                }
        };

        var workoutLogs = new List<WorkoutLog> { workoutLog }.AsQueryable().BuildMockDbSet();
        _contextMock.Setup(x => x.WorkoutLogs).Returns(workoutLogs.Object);
        _contextMock.Setup(x => x.WorkoutLogs.FindAsync(It.IsAny<object[]>()))
                    .ReturnsAsync(workoutLog);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Success.Should().BeTrue();
        var refreshedWorkoutLog = await _contextMock.Object.WorkoutLogs.FindAsync(new object[] { 1 });
        //refreshedWorkoutLog?.ExerciseLogs.Should().BeEmpty();

        _contextMock.Verify(x => x.ExerciseLogs.Remove(It.IsAny<ExerciseLog>()), Times.Once);
        _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Validator_Should_Pass_When_Command_Is_Valid()
    {
        // Arrange
        var command = new UpdateWorkoutLogCommand
        {
            WorkoutLogId = 1,
            Note = "Valid note",
            Duration = new TimeOnly(1, 30),
            ExerciseLogs = new List<UpdateExerciseLogCommand>
                {
                    new UpdateExerciseLogCommand
                    {
                        ExerciseLogId = 1,
                        ExerciseId = 1,
                        OrderInSession = 1,
                        Note = "Valid exercise note",
                        NumberOfSets = 3,
                        WeightsUsedValue = new List<int> { 100, 100, 100 },
                        NumberOfRepsValue = new List<int> { 10, 10, 10 },
                        FootageUrls = "[\"https://example.com/footage1\",\"https://example.com/footage2\"]"
                    }
                }
        };

        // Act
        var validationResult = await _validator.ValidateAsync(command);

        // Assert
        validationResult.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task Validator_Should_Fail_When_WorkoutLogId_Is_Empty()
    {
        // Arrange
        var command = new UpdateWorkoutLogCommand
        {
            WorkoutLogId = 0,
            Note = "Valid note",
            Duration = new TimeOnly(1, 30),
            ExerciseLogs = new List<UpdateExerciseLogCommand>
                {
                    new UpdateExerciseLogCommand
                    {
                        ExerciseLogId = 1,
                        ExerciseId = 1,
                        OrderInSession = 1,
                        Note = "Valid exercise note",
                        NumberOfSets = 3,
                        WeightsUsedValue = new List<int> { 100, 100, 100 },
                        NumberOfRepsValue = new List<int> { 10, 10, 10 },
                        FootageUrls = "[\"https://example.com/footage1\",\"https://example.com/footage2\"]"
                    }
                }
        };

        // Act
        var validationResult = await _validator.ValidateAsync(command);

        // Assert
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Should().ContainSingle(error => error.PropertyName == "WorkoutLogId");
    }
}
