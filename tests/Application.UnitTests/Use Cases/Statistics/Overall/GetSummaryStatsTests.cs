using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitLog.Application.Common.Interfaces;
using FitLog.Application.Statistics_Workout.Queries.GetSummaryStats;
using FitLog.Application.WorkoutLogs.Queries.GetWorkoutHistory;
using FitLog.Application.WorkoutLogs.Queries.GetWorkoutLogsWithPagination;
using FluentAssertions;
using MediatR;
using Moq;
using Xunit;

namespace FitLog.Application.UnitTests.Use_Cases.Statistics.Overall;
public class GetSummaryStatsTests
{
    private readonly Mock<IApplicationDbContext> _mockContext;
    private readonly Mock<IMediator> _mockMediator;
    private readonly GetSummaryStatsQueryHandler _handler;

    public GetSummaryStatsTests()
    {
        _mockContext = new Mock<IApplicationDbContext>();
        _mockMediator = new Mock<IMediator>();
        _handler = new GetSummaryStatsQueryHandler(_mockContext.Object, _mockMediator.Object);
    }

    [Fact]
    public async Task Handle_GivenValidRequest_ReturnsSummaryStats()
    {
        // Arrange
        var userId = "user123";
        var query = new GetSummaryStatsQuery
        {
            UserId = userId,
            TimeFrame = "Monthly"
        };

        var workoutLogs = new List<WorkoutLogDTO>
            {
                new WorkoutLogDTO
                {
                    Created = new DateTimeOffset(new DateTime(2023, 7, 1)),
                    Duration = new TimeOnly(1, 30), // 1.5 hours
                    ExerciseLogs = new List<ExerciseLogDTO>
                    {
                        new ExerciseLogDTO
                        {
                            WeightsUsed = "[100, 105]",
                            NumberOfReps = "[10, 8]"
                        }
                    }
                },
                new WorkoutLogDTO
                {
                    Created = new DateTimeOffset(new DateTime(2023, 7, 15)),
                    Duration = new TimeOnly(1, 0), // 1 hour
                    ExerciseLogs = new List<ExerciseLogDTO>
                    {
                        new ExerciseLogDTO
                        {
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
        var summaryStats = result[new DateTime(2023, 7, 1)];
        summaryStats.NumberOfWorkouts.Should().Be(2);
        summaryStats.HoursAtTheGym.Should().Be(2.5); // 1.5 + 1
        summaryStats.WeightLifted.Should().Be(100 * 10 + 105 * 8 + 110 * 6); // 100*10 + 105*8 + 110*6
        summaryStats.WeekStreak.Should().Be(1); // All logs in the same month

        //result.Should().ContainKey(new DateTime(2023, 7, 15));
        //summaryStats = result[new DateTime(2023, 7, 15)];
        //summaryStats.NumberOfWorkouts.Should().Be(2);
        //summaryStats.HoursAtTheGym.Should().Be(2.5); // 1.5 + 1
        //summaryStats.WeightLifted.Should().Be(1000 + 840 + 660); // 100*10 + 105*8 + 110*6
        //summaryStats.WeekStreak.Should().Be(1); // All logs in the same month
    }

    [Fact]
    public async Task Handle_GivenNoLogsFound_ReturnsEmptyDictionary()
    {
        // Arrange
        var userId = "user123";
        var query = new GetSummaryStatsQuery
        {
            UserId = userId,
            TimeFrame = "Monthly"
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

    [Fact]
    public async Task Handle_InvalidTimeFrame_ThrowsArgumentException()
    {
        // Arrange
        var userId = "user123";
        var query = new GetSummaryStatsQuery
        {
            UserId = userId,
            TimeFrame = "InvalidTimeFrame"
        };

        // Act
        Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("Invalid TimeFrame");
    }
}
