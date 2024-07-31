using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Models;
using FitLog.Application.WorkoutLogs.Commands.CreateWorkoutLog;
using FitLog.Domain.Entities;
using Moq;
using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace FitLog.Application.UnitTests.UseCases.WorkoutLogs.Commands
{
	public class CreateWorkoutLogCommandHandlerTests
	{
        #region Setup
        private readonly Mock<IApplicationDbContext> _contextMock;
		private readonly CreateWorkoutLogCommandHandler _handler;
        private readonly CreateWorkoutLogCommandValidator _validator;

        public CreateWorkoutLogCommandHandlerTests()
		{
            // Create mock instances
            _contextMock = new Mock<IApplicationDbContext>();

            var workoutLogsDbSetMock = new Mock<DbSet<WorkoutLog>>();
            var exerciseLogsDbSetMock = new Mock<DbSet<ExerciseLog>>();

            _contextMock.Setup(x => x.WorkoutLogs).Returns(workoutLogsDbSetMock.Object);
            _contextMock.Setup(x => x.ExerciseLogs).Returns(exerciseLogsDbSetMock.Object);

            _handler = new CreateWorkoutLogCommandHandler(_contextMock.Object);
            _validator = new CreateWorkoutLogCommandValidator();
        }
        #endregion

        #region Cases
        [Fact]
        public async Task Handle_Should_Create_WorkoutLog_When_Command_Is_Valid()
        {
            // Arrange
            var command = new CreateWorkoutLogCommand("user1", new CreateWorkoutLogCommandDTO
            {
                Note = "Test workout",
                Duration = new TimeOnly(1, 30),
                ExerciseLogs = new List<CreateExerciseLogCommand>
                {
                    new CreateExerciseLogCommand
                    {
                        ExerciseId = 1,
                        OrderInSession = 1,
                        OrderInSuperset = 1,
                        Note = "Test exercise",
                        NumberOfSets = 3,
                        WeightsUsedValue = new List<int> { 100, 100, 100 },
                        NumberOfRepsValue = new List<int> { 10, 10, 10 },
                        FootageUrls = "[\"https://example.com/footage1\",\"https://example.com/footage2\"]"
                    }
                }
            });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Success.Should().BeTrue();
            _contextMock.Verify(x => x.WorkoutLogs.Add(It.IsAny<WorkoutLog>()), Times.Once);
            _contextMock.Verify(x => x.ExerciseLogs.Add(It.IsAny<ExerciseLog>()), Times.Once);
            _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Exactly(2));
        }

        [Fact]
        public async Task Handle_Should_Create_WorkoutLog_Without_ExerciseLogs_When_ExerciseLogs_Is_Null()
        {
            // Arrange
            var command = new CreateWorkoutLogCommand("user1", new CreateWorkoutLogCommandDTO
            {
                Note = "Test workout",
                Duration = new TimeOnly(1, 30),
                ExerciseLogs = null
            });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Success.Should().BeTrue();
            _contextMock.Verify(x => x.WorkoutLogs.Add(It.IsAny<WorkoutLog>()), Times.Once);
            _contextMock.Verify(x => x.ExerciseLogs.Add(It.IsAny<ExerciseLog>()), Times.Never);
            _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Exactly(2));
        }

        [Fact]
        public async Task Handle_Should_Create_WorkoutLog_Without_ExerciseLogs_When_ExerciseLogs_Is_Empty()
        {
            // Arrange
            var command = new CreateWorkoutLogCommand("user1", new CreateWorkoutLogCommandDTO
            {
                Note = "Test workout",
                Duration = new TimeOnly(1, 30),
                ExerciseLogs = new List<CreateExerciseLogCommand>()
            });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Success.Should().BeTrue();
            _contextMock.Verify(x => x.WorkoutLogs.Add(It.IsAny<WorkoutLog>()), Times.Once);
            _contextMock.Verify(x => x.ExerciseLogs.Add(It.IsAny<ExerciseLog>()), Times.Never);
            _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Exactly(2));
        }

        [Fact]
        public async Task Handle_Should_Fail_When_Context_SaveChangesAsync_Throws_Exception()
        {
            // Arrange
            var command = new CreateWorkoutLogCommand("user1", new CreateWorkoutLogCommandDTO
            {
                Note = "Test workout",
                Duration = new TimeOnly(1, 30),
                ExerciseLogs = new List<CreateExerciseLogCommand>
                {
                    new CreateExerciseLogCommand
                    {
                        ExerciseId = 1,
                        OrderInSession = 1,
                        OrderInSuperset = 1,
                        Note = "Test exercise",
                        NumberOfSets = 3,
                        WeightsUsedValue = new List<int> { 100, 100, 100 },
                        NumberOfRepsValue = new List<int> { 10, 10, 10 },
                        FootageUrls = "[\"https://example.com/footage1\",\"https://example.com/footage2\"]"
                    }
                }
            });

            _contextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Success.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e == "Database error");
        }

        [Fact]
        public async Task Validator_Should_Pass_When_Command_Is_Valid()
        {
            // Arrange
            var command = new CreateWorkoutLogCommand("user1", new CreateWorkoutLogCommandDTO
            {
                Note = "Valid note",
                Duration = new TimeOnly(1, 0),
                ExerciseLogs = new List<CreateExerciseLogCommand>
                {
                    new CreateExerciseLogCommand
                    {
                        ExerciseId = 1,
                        OrderInSession = 1,
                        Note = "Valid exercise note",
                        NumberOfSets = 3,
                        WeightsUsedValue = new List<int> { 100, 100, 100 },
                        NumberOfRepsValue = new List<int> { 10, 10, 10 },
                        FootageUrls = "[\"https://example.com/footage1\",\"https://example.com/footage2\"]"
                    }
                }
            });

            // Act
            var validationResult = await _validator.ValidateAsync(command);

            // Assert
            validationResult.IsValid.Should().BeTrue();
        }

        [Fact]
        public async Task Validator_Should_Fail_When_CreatedBy_Is_Null()
        {
            // Arrange
            var command = new CreateWorkoutLogCommand(null, new CreateWorkoutLogCommandDTO
            {
                Note = "Valid note",
                Duration = new TimeOnly(1, 0),
                ExerciseLogs = new List<CreateExerciseLogCommand>
                {
                    new CreateExerciseLogCommand
                    {
                        ExerciseId = 1,
                        OrderInSession = 1,
                        Note = "Valid exercise note",
                        NumberOfSets = 3,
                        WeightsUsedValue = new List<int> { 100, 100, 100 },
                        NumberOfRepsValue = new List<int> { 10, 10, 10 },
                        FootageUrls = "[\"https://example.com/footage1\",\"https://example.com/footage2\"]"
                    }
                }
            });

            // Act
            var validationResult = await _validator.ValidateAsync(command);

            // Assert
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().ContainSingle(error => error.PropertyName == "CreatedBy");
        }

        [Fact]
        public async Task Validator_Should_Fail_When_Note_Is_Too_Long()
        {
            // Arrange
            var command = new CreateWorkoutLogCommand("user1", new CreateWorkoutLogCommandDTO
            {
                Note = new string('a', 501), // Note exceeds the max length (assuming max length is 500)
                Duration = new TimeOnly(1, 0),
                ExerciseLogs = new List<CreateExerciseLogCommand>
                {
                    new CreateExerciseLogCommand
                    {
                        ExerciseId = 1,
                        OrderInSession = 1,
                        Note = "Valid exercise note",
                        NumberOfSets = 3,
                        WeightsUsedValue = new List<int> { 100, 100, 100 },
                        NumberOfRepsValue = new List<int> { 10, 10, 10 },
                        FootageUrls = "[\"https://example.com/footage1\",\"https://example.com/footage2\"]"
                    }
                }
            });

            // Act
            var validationResult = await _validator.ValidateAsync(command);

            // Assert
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().ContainSingle(error => error.PropertyName == "Note");
        }

        [Fact]
        public async Task Validator_Should_Fail_When_ExerciseId_Is_Null()
        {
            // Arrange
            var command = new CreateWorkoutLogCommand("user1", new CreateWorkoutLogCommandDTO
            {
                Note = "Valid note",
                Duration = new TimeOnly(1, 0),
                ExerciseLogs = new List<CreateExerciseLogCommand>
                {
                    new CreateExerciseLogCommand
                    {
                        ExerciseId = null,
                        OrderInSession = 1,
                        Note = "Valid exercise note",
                        NumberOfSets = 3,
                        WeightsUsedValue = new List<int> { 100, 100, 100 },
                        NumberOfRepsValue = new List<int> { 10, 10, 10 },
                        FootageUrls = "[\"https://example.com/footage1\",\"https://example.com/footage2\"]"
                    }
                }
            });

            // Act
            var validationResult = await _validator.ValidateAsync(command);

            // Assert
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().ContainSingle(error => error.PropertyName == "ExerciseLogs[0].ExerciseId");
        }

        [Fact]
        public async Task Validator_Should_Fail_When_FootageUrls_Is_Invalid()
        {
            // Arrange
            var command = new CreateWorkoutLogCommand("user1", new CreateWorkoutLogCommandDTO
            {
                Note = "Valid note",
                Duration = new TimeOnly(1, 0),
                ExerciseLogs = new List<CreateExerciseLogCommand>
                {
                    new CreateExerciseLogCommand
                    {
                        ExerciseId = 1,
                        OrderInSession = 1,
                        Note = "Valid exercise note",
                        NumberOfSets = 3,
                        WeightsUsedValue = new List<int> { 100, 100, 100 },
                        NumberOfRepsValue = new List<int> { 10, 10, 10 },
                        FootageUrls = "invalid_json_format"
                    }
                }
            });

            // Act
            var validationResult = await _validator.ValidateAsync(command);

            // Assert
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().ContainSingle(error => error.PropertyName == "ExerciseLogs[0].FootageUrls");
        }
        #endregion
    }
}
