using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FitLog.Application.Common.Interfaces;
using FitLog.Application.Exercises.Queries.GetExercises;
using FitLog.Application.Use_Cases.WorkoutTemplates.Queries.GetWorkoutTemplateDetails;
using FitLog.Application.WorkoutTemplates.Queries.GetWorkoutTemplateDetails;
using FitLog.Domain.Entities;
using FitLog.Infrastructure.Identity;
using FluentAssertions;
using MockQueryable.Moq;
using Moq;
using Xunit;

namespace FitLog.Application.UnitTests.Use_Cases.WorkoutTemplates.Queries;
public class GetWorkoutTemplateDetailsQueryHandlerTests
{
    private readonly Mock<IApplicationDbContext> _mockContext;
    private readonly Mock<IMapper> _mockMapper;
    private readonly GetWorkoutTemplateDetailsQueryHandler _handler;

    public GetWorkoutTemplateDetailsQueryHandlerTests()
    {
        _mockContext = new Mock<IApplicationDbContext>();
        _mockMapper = new Mock<IMapper>();
        _handler = new GetWorkoutTemplateDetailsQueryHandler(_mockContext.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task Handle_GivenValidRequest_ReturnsWorkoutTemplateDetails()
    {
        // Arrange
        var query = new GetWorkoutTemplateDetailsQuery { Id = 1 };

        var workoutTemplates = new List<WorkoutTemplate>
            {
                new WorkoutTemplate
                {
                    Id = 1,
                    TemplateName = "Template1",
                    Duration = "01:00",
                    CreatedByNavigation = new AspNetUser { UserName = "Creator1" },
                    WorkoutTemplateExercises = new List<WorkoutTemplateExercise>
                    {
                        new WorkoutTemplateExercise
                        {
                            Exercise = new Exercise { ExerciseName = "Exercise1" }
                        }
                    }
                }
            }.AsQueryable().BuildMockDbSet();

        _mockContext.Setup(x => x.WorkoutTemplates).Returns(workoutTemplates.Object);

        var workoutTemplateDto = new WorkoutTemplateDetailsDto
        {
            Id = 1,
            TemplateName = "Template1",
            Duration = "01:00",
            CreatorName = "Creator1",
            WorkoutTemplateExercises = new List<WorkoutTemplateExerciseDTO>
                {
                    new WorkoutTemplateExerciseDTO
                    {
                        Exercise = new ExerciseDTO { ExerciseName = "Exercise1" }
                    }
                }
        };

        _mockMapper.Setup(m => m.Map<WorkoutTemplateDetailsDto>(It.IsAny<WorkoutTemplate>()))
            .Returns(workoutTemplateDto);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(1);
        result.TemplateName.Should().Be("Template1");
        result.Duration.Should().Be("01:00");
        result.CreatorName.Should().Be("Creator1");
        result.WorkoutTemplateExercises.Should().HaveCount(1);
        result.WorkoutTemplateExercises?.First()?.Exercise?.ExerciseName.Should().Be("Exercise1");
    }

    [Fact]
    public async Task Handle_GivenNonExistentTemplate_ReturnsNull()
    {
        // Arrange
        var query = new GetWorkoutTemplateDetailsQuery { Id = 2 };

        var workoutTemplates = new List<WorkoutTemplate>().AsQueryable().BuildMockDbSet();

        _mockContext.Setup(x => x.WorkoutTemplates).Returns(workoutTemplates.Object);

        //_mockMapper.Setup(m => m.Map<WorkoutTemplateDetailsDto>(It.IsAny<WorkoutTemplate>()))
        //    .Returns((WorkoutTemplateDetailsDto)null);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task Handle_InvalidTemplateId_ValidationReturnsFalse()
    {
        // Arrange
        var query = new GetWorkoutTemplateDetailsQuery { Id = 0 }; // Invalid ID

        var validator = new GetWorkoutTemplateDetailsQueryValidator();
        var validationResult = await validator.ValidateAsync(query);

        // Assert
        validationResult.IsValid.Should().BeFalse();
    }
}
