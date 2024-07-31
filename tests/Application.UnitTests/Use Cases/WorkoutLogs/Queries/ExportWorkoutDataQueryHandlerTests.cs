using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FitLog.Application.Common.Interfaces;
using FitLog.Application.WorkoutLogs.Queries.ExportWorkoutData;
using FitLog.Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using MockQueryable.Moq;
using Xunit;

namespace FitLog.Application.UnitTests.WorkoutLogs.Queries.ExportWorkoutData
{
    public class ExportWorkoutDataQueryHandlerTests
    {
        private readonly Mock<IApplicationDbContext> _mockContext;
        private readonly ExportWorkoutDataQueryHandler _handler;

        public ExportWorkoutDataQueryHandlerTests()
        {
            _mockContext = new Mock<IApplicationDbContext>();
            _handler = new ExportWorkoutDataQueryHandler(_mockContext.Object);
        }

        [Fact]
        public async Task Handle_GivenValidRequest_ReturnsFormattedWorkoutData()
        {
            // Arrange
            var userId = "user123";
            var startDate = new DateTime(2023, 7, 1);
            var endDate = new DateTime(2023, 7, 7);

            var query = new ExportWorkoutDataQuery(userId, startDate, endDate);

            var workoutLogs = new List<WorkoutLog>
            {
                new WorkoutLog
                {
                    Created = new DateTime(2023, 7, 1),
                    Note = "Workout note",
                    CreatedBy = userId,
                    ExerciseLogs = new List<ExerciseLog>
                    {
                        new ExerciseLog
                        {
                            Exercise = new Exercise { ExerciseName = "Squat" },
                            OrderInSession = 1,
                            NumberOfSets = 3,
                            WeightsUsedValue = new List<int> { 100, 105, 110 },
                            NumberOfRepsValue = new List<int> { 10, 8, 6 },
                            Note = "Exercise note"
                        }
                    }
                }
            }.AsQueryable().BuildMockDbSet();

            _mockContext.Setup(x => x.WorkoutLogs).Returns(workoutLogs.Object);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            var expectedOutput = "Date,Note,Exercise,Order,Sets,Weight,Reps,ExerciseNote" + Environment.NewLine +
                                 "2023-07-01,Workout note,Squat,1,3,100kgx10 / 105kgx8 / 110kgx6,Exercise note";

            result.Should().Be(expectedOutput);
        }

        [Fact]
        public async Task Handle_GivenNoWorkoutLogs_ReturnsHeaderOnly()
        {
            // Arrange
            var userId = "user123";
            var startDate = new DateTime(2023, 7, 1);
            var endDate = new DateTime(2023, 7, 7);

            var query = new ExportWorkoutDataQuery(userId, startDate, endDate);

            var workoutLogs = new List<WorkoutLog>().AsQueryable().BuildMockDbSet();

            _mockContext.Setup(x => x.WorkoutLogs).Returns(workoutLogs.Object);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            var expectedOutput = "Date,Note,Exercise,Order,Sets,Weight,Reps,ExerciseNote";

            result.Should().Be(expectedOutput);
        }

        [Fact]
        public async Task Handle_GivenValidRequestWithMultipleWorkoutLogs_ReturnsFormattedWorkoutData()
        {
            // Arrange
            var userId = "user123";
            var startDate = new DateTime(2023, 7, 1);
            var endDate = new DateTime(2023, 7, 7);

            var query = new ExportWorkoutDataQuery(userId, startDate, endDate);

            var workoutLogs = new List<WorkoutLog>
            {
                new WorkoutLog
                {
                    Created = new DateTime(2023, 7, 1),
                    Note = "Workout note 1",
                    CreatedBy = userId,
                    ExerciseLogs = new List<ExerciseLog>
                    {
                        new ExerciseLog
                        {
                            Exercise = new Exercise { ExerciseName = "Squat" },
                            OrderInSession = 1,
                            NumberOfSets = 2,
                            WeightsUsedValue = new List<int> { 100, 105 },
                            NumberOfRepsValue = new List<int> { 10, 8 },
                            Note = "Exercise note 1"
                        }
                    }
                },
                new WorkoutLog
                {
                    Created = new DateTime(2023, 7, 2),
                    Note = "Workout note 2",
                    CreatedBy = userId,
                    ExerciseLogs = new List<ExerciseLog>
                    {
                        new ExerciseLog
                        {
                            Exercise = new Exercise { ExerciseName = "Bench Press" },
                            OrderInSession = 1,
                            NumberOfSets = 3,
                            WeightsUsedValue = new List<int> { 80, 85, 90 },
                            NumberOfRepsValue = new List<int> { 10, 8, 6 },
                            Note = "Exercise note 2"
                        }
                    }
                }
            }.AsQueryable().BuildMockDbSet();

            _mockContext.Setup(x => x.WorkoutLogs).Returns(workoutLogs.Object);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            var expectedOutput = "Date,Note,Exercise,Order,Sets,Weight,Reps,ExerciseNote" + Environment.NewLine +
                                 "2023-07-01,Workout note 1,Squat,1,2,100kgx10 / 105kgx8,Exercise note 1" + Environment.NewLine +
                                 "2023-07-02,Workout note 2,Bench Press,1,3,80kgx10 / 85kgx8 / 90kgx6,Exercise note 2";

            result.Should().Be(expectedOutput);
        }

        [Fact]
        public async Task Handle_GivenWorkoutLogWithNoWeightsOrReps_ReturnsFormattedWorkoutData()
        {
            // Arrange
            var userId = "user123";
            var startDate = new DateTime(2023, 7, 1);
            var endDate = new DateTime(2023, 7, 7);

            var query = new ExportWorkoutDataQuery(userId, startDate, endDate);

            var workoutLogs = new List<WorkoutLog>
            {
                new WorkoutLog
                {
                    Created = new DateTime(2023, 7, 1),
                    Note = "Workout note",
                    CreatedBy = userId,
                    ExerciseLogs = new List<ExerciseLog>
                    {
                        new ExerciseLog
                        {
                            Exercise = new Exercise { ExerciseName = "Deadlift" },
                            OrderInSession = 1,
                            NumberOfSets = 3,
                            WeightsUsedValue = new List<int>(), // No weights
                            NumberOfRepsValue = new List<int>(), // No reps
                            Note = "Exercise note"
                        }
                    }
                }
            }.AsQueryable().BuildMockDbSet();

            _mockContext.Setup(x => x.WorkoutLogs).Returns(workoutLogs.Object);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            var expectedOutput = "Date,Note,Exercise,Order,Sets,Weight,Reps,ExerciseNote" + Environment.NewLine +
                                 "2023-07-01,Workout note,Deadlift,1,3,No weightxNo reps / No weightxNo reps / No weightxNo reps,Exercise note";

            result.Should().Be(expectedOutput);
        }

        [Fact]
        public async Task Handle_GivenWorkoutLogWithEmptyExerciseLogs_ReturnsFormattedWorkoutData()
        {
            // Arrange
            var userId = "user123";
            var startDate = new DateTime(2023, 7, 1);
            var endDate = new DateTime(2023, 7, 7);

            var query = new ExportWorkoutDataQuery(userId, startDate, endDate);

            var workoutLogs = new List<WorkoutLog>
            {
                new WorkoutLog
                {
                    Created = new DateTime(2023, 7, 1),
                    Note = "Workout note",
                    CreatedBy = userId,
                    ExerciseLogs = new List<ExerciseLog>() // No exercise logs
                }
            }.AsQueryable().BuildMockDbSet();

            _mockContext.Setup(x => x.WorkoutLogs).Returns(workoutLogs.Object);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            var expectedOutput = "Date,Note,Exercise,Order,Sets,Weight,Reps,ExerciseNote";

            result.Should().Be(expectedOutput);
        }
    }
}
