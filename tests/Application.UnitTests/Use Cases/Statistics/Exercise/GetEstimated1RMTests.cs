using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Moq;
using Xunit;
using FitLog.Application.Common.Interfaces;
using FitLog.Application.Statistics_Exercise.Queries.GetExerciseEstimated1RMs;
using FitLog.Application.Statistics_Exercise.Queries.GetExerciseLogHistory;
using FitLog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Ardalis.GuardClauses;
using FitLog.Application.WorkoutLogs.Queries.GetWorkoutLogsWithPagination;
using Azure.Core;
using MockQueryable.Moq;

namespace FitLog.Application.UnitTests.Use_Cases.Statistics.Exercise
{
    public class GetExerciseEstimated1RMsQueryHandlerTests
    {
        private readonly Mock<IApplicationDbContext> _mockContext;
        private readonly Mock<IMediator> _mockMediator;
        private GetExerciseEstimated1RMsQueryHandler _handler;

        public GetExerciseEstimated1RMsQueryHandlerTests()
        {
            _mockContext = new Mock<IApplicationDbContext>();
            _mockMediator = new Mock<IMediator>();
            _handler = new GetExerciseEstimated1RMsQueryHandler(_mockContext.Object, _mockMediator.Object);
        }

        [Fact]
        public async Task Handle_GivenValidRequest_ReturnsEstimated1RMs()
        {
            // Arrange
            var userId = "user123";
            var query = new GetExerciseEstimated1RMsQuery
            {
                UserId = userId,
                ExerciseId = 1
            };

            var exerciseLogs = new List<ExerciseLogDTO>
            {
                new ExerciseLogDTO
                {
                    DateCreated = new DateTime(2023, 7, 1),
                    WeightsUsed = "[100, 105]",
                    NumberOfReps = "[10, 8]"
                }
            }.AsEnumerable();

            _mockMediator
                .Setup(m => m.Send(It.IsAny<GetExerciseLogHistoryQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(exerciseLogs);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().ContainKey(new DateTime(2023, 7, 1));
            var oneRepMaxRecord = result[new DateTime(2023, 7, 1)];
            oneRepMaxRecord.Epley.Should().BeGreaterThan(0);
            oneRepMaxRecord.Brzycki.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task Handle_GivenNoLogsFound_ThrowsNotFoundException()
        {
            // Arrange
            var userId = "user123";
            var query = new GetExerciseEstimated1RMsQuery
            {
                UserId = userId,
                ExerciseId = 1
            };

            var exerciseLogs = Enumerable.Empty<ExerciseLogDTO>().AsEnumerable();

            _mockMediator
                .Setup(m => m.Send(It.IsAny<GetExerciseLogHistoryQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(exerciseLogs);

            // Act
            Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task Handle_GivenLogsWithSameDate_CombinesMax1RMs()
        {
            // Arrange
            var userId = "user123";
            var query = new GetExerciseEstimated1RMsQuery
            {
                UserId = userId,
                ExerciseId = 1
            };

            var exerciseLogs = new List<ExerciseLogDTO>
            {
                new ExerciseLogDTO
                {
                    DateCreated = new DateTime(2023, 7, 1),
                    WeightsUsed = "[100]",
                    NumberOfReps = "[10]"
                },
                new ExerciseLogDTO
                {
                    DateCreated = new DateTime(2023, 7, 1),
                    WeightsUsed = "[105]",
                    NumberOfReps = "[8]"
                }
            }.AsEnumerable();

            _mockMediator
                .Setup(m => m.Send(It.IsAny<GetExerciseLogHistoryQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(exerciseLogs);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().ContainKey(new DateTime(2023, 7, 1));
            var oneRepMaxRecord = result[new DateTime(2023, 7, 1)];
            oneRepMaxRecord.Epley.Should().BeGreaterThan(0);
            oneRepMaxRecord.Brzycki.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task Handle_GivenLogsWithDifferentDates_ReturnsSeparate1RMs()
        {
            // Arrange
            var userId = "user123";
            var query = new GetExerciseEstimated1RMsQuery
            {
                UserId = userId,
                ExerciseId = 1
            };

            var exerciseLogs = new List<ExerciseLogDTO>
            {
                new ExerciseLogDTO
                {
                    DateCreated = new DateTime(2023, 7, 1),
                    WeightsUsed = "[100]",
                    NumberOfReps = "[10]"
                },
                new ExerciseLogDTO
                {
                    DateCreated = new DateTime(2023, 7, 2),
                    WeightsUsed = "[105]",
                    NumberOfReps = "[8]"
                }
            }.AsEnumerable();

            _mockMediator
                .Setup(m => m.Send(It.IsAny<GetExerciseLogHistoryQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(exerciseLogs);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().ContainKey(new DateTime(2023, 7, 1));
            result.Should().ContainKey(new DateTime(2023, 7, 2));
        }
        [Fact]
        public async Task Handle_GivenEmptyWeightsAndReps_ReturnsDefault1RMs()
        {
            // Arrange
            var userId = "user123";
            // Arrange
            var query = new GetExerciseEstimated1RMsQuery
            {
                UserId = userId,
                ExerciseId = 1
            };

            var exerciseLogs = new List<ExerciseLogDTO>
            {
                new ExerciseLogDTO
                {
                    DateCreated = new DateTime(2023, 7, 1),
                    WeightsUsed = "[]",
                    NumberOfReps ="[]"
                }
            }.AsEnumerable();

            _mockMediator
                .Setup(m => m.Send(It.IsAny<GetExerciseLogHistoryQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(exerciseLogs);

            _handler = new GetExerciseEstimated1RMsQueryHandler(_mockContext.Object, _mockMediator.Object);
            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().ContainKey(new DateTime(2023, 7, 1));
            var oneRepMaxRecord = result[new DateTime(2023, 7, 1)];
            oneRepMaxRecord.Epley.Should().Be(double.MinValue);
            oneRepMaxRecord.Brzycki.Should().Be(double.MinValue);
        }
    }
}
