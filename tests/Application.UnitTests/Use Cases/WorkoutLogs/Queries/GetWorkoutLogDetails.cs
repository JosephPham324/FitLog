using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Moq;
using Xunit;
using FitLog.Application.Common.Interfaces;
using FitLog.Application.WorkoutLogs.Queries.GetWorkoutLogDetails;
using FitLog.Domain.Entities;
using MockQueryable.Moq;
using Microsoft.EntityFrameworkCore;
using FitLog.Application.WorkoutLogs.Queries.GetWorkoutLogsWithPagination;
using Ardalis.GuardClauses;

namespace FitLog.Application.UnitTests.Use_Cases.WorkoutLogs.Queries;
public class GetWorkoutLogDetailsQueryHandlerTests
{
    private readonly Mock<IApplicationDbContext> _mockContext;
    private readonly Mock<IMapper> _mockMapper;
    private readonly GetWorkoutLogDetailsQueryHandler _handler;

    public GetWorkoutLogDetailsQueryHandlerTests()
    {
        _mockContext = new Mock<IApplicationDbContext>();
        _mockMapper = new Mock<IMapper>();
        _handler = new GetWorkoutLogDetailsQueryHandler(_mockContext.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task Handle_GivenValidRequest_ReturnsWorkoutLogDetails()
    {
        // Arrange
        var userId = "user123";
        var workoutLogId = 1;
        var query = new GetWorkoutLogDetailsQuery
        {
            UserId = userId,
            WorkoutLogId = workoutLogId
        };

        var workoutLog = new WorkoutLog
        {
            Id = workoutLogId,
            WorkoutLogName = "Workout1",
            Note = "Workout note",
            CreatedBy = userId,
            Created = DateTimeOffset.UtcNow,
            LastModified = DateTimeOffset.UtcNow,
            ExerciseLogs = new List<ExerciseLog>
                {
                    new ExerciseLog
                    {
                        Exercise = new Exercise { ExerciseName = "Squat" },
                        OrderInSession = 1,
                        Note = "Exercise note"
                    }
                }
        };

        var workoutLogs = new List<WorkoutLog> { workoutLog }.AsQueryable().BuildMockDbSet();
        _mockContext.Setup(x => x.WorkoutLogs).Returns(workoutLogs.Object);

        var workoutLogDto = new WorkoutLogDetailsDto
        {
            Id = workoutLogId,
            WorkoutLogName = "Workout1",
            Note = "Workout note",
            CreatedBy = userId,
            Created = DateTimeOffset.UtcNow,
            LastModified = DateTimeOffset.UtcNow,
            ExerciseLogs = new List<ExerciseLogDTO>
                {
                    new ExerciseLogDTO { ExerciseName = "Squat", Note = "Exercise note" }
                }
        };

        _mockMapper.Setup(m => m.Map<WorkoutLogDetailsDto>(It.IsAny<WorkoutLog>())).Returns(workoutLogDto);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(workoutLogId);
        result.WorkoutLogName.Should().Be("Workout1");
        result.Note.Should().Be("Workout note");
        result.ExerciseLogs.Should().HaveCount(1);
        result.ExerciseLogs.First().ExerciseName.Should().Be("Squat");
    }

    [Fact]
    public async Task Handle_GivenNonExistentWorkoutLog_ThrowsNotFoundException()
    {
        // Arrange
        var userId = "user123";
        var workoutLogId = 2; // Non-existent ID
        var query = new GetWorkoutLogDetailsQuery
        {
            UserId = userId,
            WorkoutLogId = workoutLogId
        };

        var workoutLogs = new List<WorkoutLog>().AsQueryable().BuildMockDbSet();
        _mockContext.Setup(x => x.WorkoutLogs).Returns(workoutLogs.Object);

        // Act
        Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Handle_GivenUnauthorizedUser_ThrowsUnauthorizedAccessException()
    {
        // Arrange
        var userId = "user123";
        var workoutLogId = 1;
        var query = new GetWorkoutLogDetailsQuery
        {
            UserId = userId,
            WorkoutLogId = workoutLogId
        };

        var workoutLog = new WorkoutLog
        {
            Id = workoutLogId,
            WorkoutLogName = "Workout1",
            Note = "Workout note",
            CreatedBy = "anotherUser", // Unauthorized user
            Created = DateTimeOffset.UtcNow,
            LastModified = DateTimeOffset.UtcNow,
            ExerciseLogs = new List<ExerciseLog>
                {
                    new ExerciseLog
                    {
                        Exercise = new Exercise { ExerciseName = "Squat" },
                        OrderInSession = 1,
                        Note = "Exercise note"
                    }
                }
        };

        var workoutLogs = new List<WorkoutLog> { workoutLog }.AsQueryable().BuildMockDbSet();
        _mockContext.Setup(x => x.WorkoutLogs).Returns(workoutLogs.Object);

        // Act
        Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task Handle_InvalidWorkoutLogId_ValidationReturnsFalse()
    {
        // Arrange
        var query = new GetWorkoutLogDetailsQuery
        {
            UserId = "user123",
            WorkoutLogId = 0 // Invalid ID
        };

        var validator = new GetWorkoutLogDetailsQueryValidator();
        var validationResult = await validator.ValidateAsync(query);

        // Assert
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Should().ContainSingle(e => e.PropertyName == "WorkoutLogId");
    }
}
