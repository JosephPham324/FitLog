using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Moq;
using Xunit;
using FitLog.Application.Common.Interfaces;
using FitLog.Application.WorkoutLogs.Queries.GetWorkoutHistory;
using FitLog.Domain.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using FitLog.Application.WorkoutLogs.Queries.GetWorkoutLogsWithPagination;

namespace FitLog.Application.UnitTests.Use_Cases.WorkoutLogs.Queries;
public class GetWorkoutHistoryQueryHandlerTests
{
    private readonly Mock<IApplicationDbContext> _mockContext;
    private readonly Mock<IMapper> _mockMapper;
    private readonly GetWorkoutHistoryQueryHandler _handler;

    public GetWorkoutHistoryQueryHandlerTests()
    {
        _mockContext = new Mock<IApplicationDbContext>();
        _mockMapper = new Mock<IMapper>();
        _handler = new GetWorkoutHistoryQueryHandler(_mockContext.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task Handle_GivenValidRequest_ReturnsWorkoutHistory()
    {
        // Arrange
        var userId = "user123";
        var startDate = new DateTime(2023, 7, 1);
        var endDate = new DateTime(2023, 7, 7);
        var query = new GetWorkoutHistoryQuery(userId, startDate, endDate);

        var workoutLogs = new List<WorkoutLog>
            {
                new WorkoutLog
                {
                    CreatedBy = userId,
                    Created = new DateTime(2023, 7, 2),
                    ExerciseLogs = new List<ExerciseLog>
                    {
                        new ExerciseLog
                        {
                            Exercise = new Exercise
                            {
                                ExerciseName = "Squat"
                            }
                        }
                    }
                }
            };

        _mockContext.Setup(x => x.WorkoutLogs)
            .Returns(workoutLogs.AsQueryable().BuildMockDbSet().Object);

        var workoutLogDTOs = new List<WorkoutLogDTO>
            {
                new WorkoutLogDTO
                {
                    Created = new DateTimeOffset(new DateTime(2023, 7, 2)),
                    ExerciseLogs = new List<ExerciseLogDTO>
                    {
                        new ExerciseLogDTO
                        {
                            ExerciseName = "Squat"
                        }
                    }
                }
            };

        _mockMapper.Setup(m => m.Map<List<WorkoutLogDTO>>(It.IsAny<List<WorkoutLog>>()))
            .Returns(workoutLogDTOs);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result.First().ExerciseLogs.First().ExerciseName.Should().Be("Squat");
    }

    [Fact]
    public async Task Handle_GivenNoWorkoutLogs_ReturnsEmptyList()
    {
        // Arrange
        var userId = "user123";
        var startDate = new DateTime(2023, 7, 1);
        var endDate = new DateTime(2023, 7, 7);
        var query = new GetWorkoutHistoryQuery(userId, startDate, endDate);

        var workoutLogs = new List<WorkoutLog>();

        _mockContext.Setup(x => x.WorkoutLogs)
            .Returns(workoutLogs.AsQueryable().BuildMockDbSet().Object);

        _mockMapper.Setup(m => m.Map<List<WorkoutLogDTO>>(It.IsAny<List<WorkoutLog>>()))
            .Returns(new List<WorkoutLogDTO>());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_GivenInvalidUserId_ThrowsValidationException()
    {
        // Arrange
        var query = new GetWorkoutHistoryQuery(null, null, null);
        var workoutLogs = new List<WorkoutLog>();

        _mockContext.Setup(x => x.WorkoutLogs)
            .Returns(workoutLogs.AsQueryable().BuildMockDbSet().Object);
        // Act
        Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
    }
}
