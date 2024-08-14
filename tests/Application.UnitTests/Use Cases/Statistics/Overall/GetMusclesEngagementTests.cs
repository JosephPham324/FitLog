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
using FitLog.Application.Statistics_Workout.Queries.GetMuscleEngagement;
using FitLog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using FitLog.Application.WorkoutLogs.Queries.GetWorkoutHistory;
using FitLog.Application.WorkoutLogs.Queries.GetWorkoutLogsWithPagination;
using MockQueryable.Moq;

namespace FitLog.Application.UnitTests.Use_Cases.Statistics.Overall;
public class GetMuscleEngagementQueryHandlerTests
{
    private readonly Mock<IApplicationDbContext> _mockContext;
    private readonly Mock<IMediator> _mockMediator;
    private readonly GetMuscleEngagementQueryHandler _handler;

    public GetMuscleEngagementQueryHandlerTests()
    {
        _mockContext = new Mock<IApplicationDbContext>();
        _mockMediator = new Mock<IMediator>();
        _handler = new GetMuscleEngagementQueryHandler(_mockContext.Object, _mockMediator.Object);
    }

    [Fact]
    public async Task Handle_GivenValidRequest_Monthly_ReturnsMuscleEngagement()
    {
        // Arrange
        var userId = "user123";
        var query = new GetMuscleEngagementQuery
        {
            UserId = userId,
            TimeFrame = "Monthly"
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
                            NumberOfSets = 3
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
                            NumberOfSets = 2
                        }
                    }
                }
            };
        var MuscleGroup = new MuscleGroup()
        {
            MuscleGroupId = 1,
            MuscleGroupName = "Biceps"
        };

        var exercise = new FitLog.Domain.Entities.Exercise
        {
            ExerciseId = 1,
            ExerciseMuscleGroups = new List<ExerciseMuscleGroup>
                {
                    new ExerciseMuscleGroup
                    {
                        ExerciseId = 1,
                        MuscleGroupId = 1,
                        MuscleGroup = MuscleGroup
                    }
                }
        };

        _mockMediator
            .Setup(m => m.Send(It.IsAny<GetWorkoutHistoryQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(workoutLogs);

        _mockContext.Setup(x => x.Exercises)

            .Returns(new List<Domain.Entities.Exercise> { exercise }.AsQueryable().BuildMockDbSet().Object);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().ContainKey(new DateTime(2023, 7, 1));
        var muscleEngagement = result[new DateTime(2023, 7, 1)];
        muscleEngagement.Should().ContainSingle(me => me.Muscle == "Biceps" && me.Sets == 5);
    }

    [Fact]
    public async Task Handle_GivenValidRequest_Weekly_ReturnsMuscleEngagement()
    {
        // Arrange
        var userId = "user123";
        var query = new GetMuscleEngagementQuery
        {
            UserId = userId,
            TimeFrame = "Weekly"
        };

        var workoutLogs = new List<WorkoutLogDTO>
            {
                new WorkoutLogDTO
                {
                    Created = new DateTimeOffset(new DateTime(2024, 7, 8)),
                    ExerciseLogs = new List<ExerciseLogDTO>
                    {
                        new ExerciseLogDTO
                        {
                            ExerciseId = 1,
                            NumberOfSets = 3
                        }
                    }
                },
                new WorkoutLogDTO
                {
                    Created = new DateTimeOffset(new DateTime(2024, 7, 15)),
                    ExerciseLogs = new List<ExerciseLogDTO>
                    {
                        new ExerciseLogDTO
                        {
                            ExerciseId = 1,
                            NumberOfSets = 2
                        }
                    }
                }
            };
        var MuscleGroup = new MuscleGroup()
        {
            MuscleGroupId = 1,
            MuscleGroupName = "Biceps"
        };

        var exercise = new FitLog.Domain.Entities.Exercise
        {
            ExerciseId = 1,
            ExerciseMuscleGroups = new List<ExerciseMuscleGroup>
                {
                    new ExerciseMuscleGroup
                    {
                        ExerciseId = 1,
                        MuscleGroupId = 1,
                        MuscleGroup = MuscleGroup
                    }
                }
        };

        _mockMediator
            .Setup(m => m.Send(It.IsAny<GetWorkoutHistoryQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(workoutLogs);

        _mockContext.Setup(x => x.Exercises)

            .Returns(new List<Domain.Entities.Exercise> { exercise }.AsQueryable().BuildMockDbSet().Object);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().ContainKey(new DateTime(2024, 7, 8));
        var muscleEngagement = result[new DateTime(2024, 7, 8)];
        muscleEngagement.Should().ContainSingle(me => me.Muscle == "Biceps" && me.Sets == 3);

        result.Should().ContainKey(new DateTime(2024, 7, 15));
        muscleEngagement = result[new DateTime(2024, 7, 15)];
        muscleEngagement.Should().ContainSingle(me => me.Muscle == "Biceps" && me.Sets == 2);
    }

    [Fact]
    public async Task Handle_GivenValidRequest_Yearly_ReturnsMuscleEngagement()
    {
        // Arrange
        var userId = "user123";
        var query = new GetMuscleEngagementQuery
        {
            UserId = userId,
            TimeFrame = "Yearly"
        };

        var workoutLogs = new List<WorkoutLogDTO>
            {
                new WorkoutLogDTO
                {
                    Created = new DateTimeOffset(new DateTime(2021, 7, 8)),
                    ExerciseLogs = new List<ExerciseLogDTO>
                    {
                        new ExerciseLogDTO
                        {
                            ExerciseId = 1,
                            NumberOfSets = 3
                        }
                    }
                },
                new WorkoutLogDTO
                {
                    Created = new DateTimeOffset(new DateTime(2024, 7, 15)),
                    ExerciseLogs = new List<ExerciseLogDTO>
                    {
                        new ExerciseLogDTO
                        {
                            ExerciseId = 1,
                            NumberOfSets = 2
                        }
                    }
                }
            };
        var MuscleGroup = new MuscleGroup()
        {
            MuscleGroupId = 1,
            MuscleGroupName = "Biceps"
        };

        var exercise = new FitLog.Domain.Entities.Exercise
        {
            ExerciseId = 1,
            ExerciseMuscleGroups = new List<ExerciseMuscleGroup>
                {
                    new ExerciseMuscleGroup
                    {
                        ExerciseId = 1,
                        MuscleGroupId = 1,
                        MuscleGroup = MuscleGroup
                    }
                }
        };

        _mockMediator
            .Setup(m => m.Send(It.IsAny<GetWorkoutHistoryQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(workoutLogs);

        _mockContext.Setup(x => x.Exercises)

            .Returns(new List<Domain.Entities.Exercise> { exercise }.AsQueryable().BuildMockDbSet().Object);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().ContainKey(new DateTime(2021, 1, 1));
        var muscleEngagement = result[new DateTime(2021, 1, 1)];
        muscleEngagement.Should().ContainSingle(me => me.Muscle == "Biceps" && me.Sets == 3);

        result.Should().ContainKey(new DateTime(2024, 1, 1));
        muscleEngagement = result[new DateTime(2024, 1 , 1)];
        muscleEngagement.Should().ContainSingle(me => me.Muscle == "Biceps" && me.Sets == 2);
    }

    [Fact]
    public async Task Handle_GivenNoLogsFound_ReturnsEmptyDictionary()
    {
        // Arrange
        var userId = "user123";
        var query = new GetMuscleEngagementQuery
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
        var query = new GetMuscleEngagementQuery
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
