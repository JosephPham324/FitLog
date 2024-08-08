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
using FitLog.Application.Statistics_Exercise.Queries.GetRecordsHistory;
using FitLog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Ardalis.GuardClauses;

namespace FitLog.Application.UnitTests.Use_Cases.Statistics.Exercise
{
    public class GetRecordHistoryTests
    {
        private readonly Mock<IApplicationDbContext> _mockContext;
        private readonly GetRecordsHistoryQueryHandler _handler;

        public GetRecordHistoryTests()
        {
            _mockContext = new Mock<IApplicationDbContext>();
            _handler = new GetRecordsHistoryQueryHandler(_mockContext.Object);
        }

        [Fact]
        public async Task Handle_GivenValidRequest_ReturnsPersonalRecordDTO()
        {
            // Arrange
            var userId = "user123";
            var query = new GetRecordsHistoryQuery
            {
                UserId = userId,
                ExerciseId = 1
            };

            var exerciseLogs = new List<ExerciseLog>
            {
                new ExerciseLog
                {
                    WorkoutLog = new WorkoutLog { CreatedBy = userId },
                    ExerciseId = 1,
                    DateCreated = new DateTime(2023, 7, 1),
                    WeightsUsed = "[100,105]",
                    NumberOfReps = "[10, 8]"
                }
            }.AsQueryable().BuildMockDbSet();

            _mockContext.Setup(x => x.ExerciseLogs)
                .Returns(exerciseLogs.Object);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Actual1RepMax.Should().Be(105);
            result.Estimated1RepMax.Should().BeGreaterThan(0);
            result.MaxVolume.Should().Be(1000 + 840);
            result.BestPerformances.Should().ContainKey(10);
            result.BestPerformances?[10].Weight.Should().Be(100);
        }

        [Fact]
        public async Task Handle_GivenNoLogsFound_ThrowsNotFoundException()
        {
            // Arrange
            var userId = "user123";
            var query = new GetRecordsHistoryQuery
            {
                UserId = userId,
                ExerciseId = 1
            };

            var exerciseLogs = Enumerable.Empty<ExerciseLog>().AsQueryable().BuildMockDbSet();

            _mockContext.Setup(x => x.ExerciseLogs)
                .Returns(exerciseLogs.Object);

            // Act
            Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>();

        }

        [Fact]
        public async Task Handle_GivenNullLogsFound_ThrowsNotFoundException()
        {
            // Arrange
            var userId = "user123";
            var query = new GetRecordsHistoryQuery
            {
                UserId = userId,
                ExerciseId = 1
            };
            var ExerciseLogs = new List<ExerciseLog>()
                .AsQueryable().BuildMockDbSet();

            _mockContext.Setup(x => x.ExerciseLogs)
                .Returns(ExerciseLogs.Object);

            // Act
            Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task Handle_GivenLogsWithBestPerformances_CalculatesBestPerformancesCorrectly()
        {
            // Arrange
            var userId = "user123";
            var query = new GetRecordsHistoryQuery
            {
                UserId = userId,
                ExerciseId = 1
            };

            var exerciseLogs = new List<ExerciseLog>
            {
                new ExerciseLog
                {
                    WorkoutLog = new WorkoutLog { CreatedBy = userId },
                    ExerciseId = 1,
                    DateCreated = new DateTime(2023, 7, 1),
                   WeightsUsed = "[100,105]",
                   NumberOfReps = "[10, 8]"
                },
                new ExerciseLog
                {
                    WorkoutLog = new WorkoutLog { CreatedBy = userId },
                    ExerciseId = 1,
                    DateCreated = new DateTime(2023, 7, 2),//110 95 6 12
                   WeightsUsed = "[110,95]",
                    NumberOfReps = "[6,12]"
                }
            }.AsQueryable().BuildMockDbSet();

            _mockContext.Setup(x => x.ExerciseLogs)
                .Returns(exerciseLogs.Object);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Actual1RepMax.Should().Be(110);
            result.Estimated1RepMax.Should().BeGreaterThan(0);
            result.MaxVolume.Should().Be(1000 + 840 + 660 + 1140);
            result.BestPerformances.Should().ContainKey(10);
            result.BestPerformances?[10].Weight.Should().Be(100);
            result.BestPerformances.Should().ContainKey(6);
            result.BestPerformances?[6].Weight.Should().Be(110);
        }
    }
}
