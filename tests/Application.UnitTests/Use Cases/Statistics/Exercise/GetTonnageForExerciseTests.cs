using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using FitLog.Application.Common.Interfaces;
using FitLog.Application.Statistics_Exercise.Queries.GetRecordsHistory;
using FitLog.Application.Statistics_Exercise.Queries.GetTotalTrainingTonnageForExercise;
using FitLog.Application.WorkoutLogs.Queries.GetWorkoutHistory;
using FitLog.Application.WorkoutLogs.Queries.GetWorkoutLogsWithPagination;
using FitLog.Domain.Entities;
using FluentAssertions;
using MediatR;
using MockQueryable.Moq;
using Moq;
using Xunit;

namespace FitLog.Application.UnitTests.Use_Cases.Statistics.Exercise;
public class GetTonnageForExerciseTests
{
    private readonly Mock<IApplicationDbContext> _mockContext;
    private readonly Mock<IMediator> _mockMediator;
    private readonly GetTotalTrainingTonnageForExerciseQueryHandler _handler;

    public GetTonnageForExerciseTests()
    {
        _mockContext = new Mock<IApplicationDbContext>();
        _mockMediator = new Mock<IMediator>();
        _handler = new GetTotalTrainingTonnageForExerciseQueryHandler(_mockContext.Object, _mockMediator.Object);
    }

    [Fact]
    public async Task Handle_GivenValidRequest_ReturnsTotalTonnage()
    {
        // Arrange
        var userId = "user123";
        var query = new GetTotalTrainingTonnageForExerciseQuery
        {
            UserId = userId,
            TimeFrame = "Monthly",
            ExerciseId = 1
        };

        var workoutLogs = new List<WorkoutLogDTO>
            {
                new WorkoutLogDTO
                {
                    Created = new DateTimeOffset(new DateTime(2023, 7, 1)),
                    ExerciseLogs = new List<ExerciseLogDTO>
                    {
                        new ExerciseLogDTO
                        {
                            ExerciseId = 1,
                            WeightsUsed = "[100, 105]",
                            NumberOfReps = "[10, 8]"
                        }
                    }
                },
                new WorkoutLogDTO
                {
                    Created = new DateTimeOffset(new DateTime(2023, 7, 15)),
                    ExerciseLogs = new List<ExerciseLogDTO>
                    {
                        new ExerciseLogDTO
                        {
                            ExerciseId = 1,
                            WeightsUsed = "[110]",
                            NumberOfReps = "[6]"
                        }
                    }
                }
            };

        _mockMediator
            .Setup(m => m.Send(It.IsAny<GetWorkoutHistoryQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(workoutLogs);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().ContainKey(new DateTime(2023, 7, 1));
        result[new DateTime(2023, 7, 1)].Should().Be(1000 + 840 + 660); // 100*10 + 105*8 + 110*6
    }

    [Fact]
    public async Task Handle_GivenNoLogsFound_ReturnsEmptyDictionary()
    {
        // Arrange
        var userId = "user123";
        var query = new GetTotalTrainingTonnageForExerciseQuery
        {
            UserId = userId,
            TimeFrame = "Monthly",
            ExerciseId = 1
        };

        var workoutLogs = new List<WorkoutLogDTO>();

        _mockMediator
            .Setup(m => m.Send(It.IsAny<GetWorkoutHistoryQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(workoutLogs);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    //[Fact]
    //public async Task Handle_InvalidTimeFrame_ThrowsArgumentException()
    //{
    //    // Arrange
    //    var userId = "user123";
    //    var query = new GetTotalTrainingTonnageForExerciseQuery
    //    {
    //        UserId = userId,
    //        TimeFrame = "InvalidTimeFrame",
    //        ExerciseId = 1
    //    };

    //    // Act
    //    Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

    //    // Assert
    //    await act.Should().ThrowAsync<ValidationException>();
    //}

    //private List<double> ParseWeightsUsed(string weightsUsed)
    //{
    //    return JsonSerializer.Deserialize<List<double>>(weightsUsed);
    //}

    //private List<int> ParseNumberOfReps(string numberOfReps)
    //{
    //    return JsonSerializer.Deserialize<List<int>>(numberOfReps);
    //}
}
