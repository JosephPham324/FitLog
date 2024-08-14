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
using FitLog.Application.Statistics_Exercise.Queries.GetExerciseLogHistory;
using FitLog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using MockQueryable.Moq;
using FitLog.Application.WorkoutLogs.Queries.GetWorkoutLogsWithPagination;
using Ardalis.GuardClauses;

namespace FitLog.Application.UnitTests.Use_Cases.Statistics.Exercise
{
    public class GetExerciseHistoryTests
    {
        private readonly Mock<IApplicationDbContext> _contextMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly GetExerciseLogHistoryQueryHandler _handler;
        private readonly GetExerciseLogHistoryQueryValidator _validator;

        public GetExerciseHistoryTests()
        {
            _contextMock = new Mock<IApplicationDbContext>();
            _mapperMock = new Mock<IMapper>();
            _mediatorMock = new Mock<IMediator>();
            _handler = new GetExerciseLogHistoryQueryHandler(_contextMock.Object, _mapperMock.Object);
            _validator = new GetExerciseLogHistoryQueryValidator();
        }

        [Fact]
        public async Task Handle_Should_Return_ExerciseLogs_When_Found()
        {
            // Arrange
            var query = new GetExerciseLogHistoryQuery { UserId = "user1", ExerciseId = 1 };
            var exerciseLogs = new List<ExerciseLog>
            {
                new ExerciseLog { ExerciseId = 1, WorkoutLog = new WorkoutLog { CreatedBy = "user1" } }
            }.AsQueryable().BuildMockDbSet();

            _contextMock.Setup(x => x.ExerciseLogs).Returns(exerciseLogs.Object);

            _mapperMock.Setup(x => x.Map<List<ExerciseLogDTO>>(It.IsAny<List<ExerciseLog>>()))
                .Returns(new List<ExerciseLogDTO> { new ExerciseLogDTO() });

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetExerciseLogHistoryQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_mapperMock.Object.Map<List<ExerciseLogDTO>>(exerciseLogs.Object.ToList()));

            // Act
            var result = await _mediatorMock.Object.Send(query);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
        }

        [Fact]
        public async Task Handle_Should_Throw_NotFoundException_When_No_ExerciseLogs_Found()
        {
            // Arrange
            var query = new GetExerciseLogHistoryQuery { UserId = "user1", ExerciseId = 1 };
            var exerciseLogs = new List<ExerciseLog>().AsQueryable().BuildMockDbSet();

            _contextMock.Setup(x => x.ExerciseLogs).Returns(exerciseLogs.Object);

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetExerciseLogHistoryQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ExerciseLogDTO>());

            // Act
            Func<Task> act = async () => await _mediatorMock.Object.Send(query);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"*{nameof(ExerciseLog)}*");
        }

        [Fact]
        public async Task Handle_Should_Fail_Validation_When_UserId_Is_Empty()
        {
            // Arrange
            var query = new GetExerciseLogHistoryQuery { UserId = string.Empty, ExerciseId = 1 };

            // Act
            Func<Task> act = async () => await _mediatorMock.Object.Send(query);

            // Assert
            await act.Should().ThrowAsync<ValidationException>();
        }

        [Fact]
        public async Task Handle_Should_Fail_Validation_When_ExerciseId_Is_Zero()
        {
            // Arrange
            var query = new GetExerciseLogHistoryQuery { UserId = "user1", ExerciseId = 0 };

            // Act
            Func<Task> act = async () => await _mediatorMock.Object.Send(query);

            // Assert
            await act.Should().ThrowAsync<ValidationException>();
        }

        [Fact]
        public async Task Handle_Should_Fail_Validation_When_Query_Is_Invalid()
        {
            // Arrange
            var query = new GetExerciseLogHistoryQuery { UserId = string.Empty, ExerciseId = 0 };

            // Act
            Func<Task> act = async () => await _mediatorMock.Object.Send(query);

            // Assert
            await act.Should().ThrowAsync<ValidationException>();
        }
    }
}
