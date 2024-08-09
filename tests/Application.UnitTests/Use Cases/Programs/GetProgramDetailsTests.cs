using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Moq;
using Xunit;
using FitLog.Application.Common.Interfaces;
using FitLog.Application.WorkoutPrograms.Queries.GetWorkoutProgramDetails;
using FitLog.Application.Use_Cases.WorkoutPrograms.DTOs;
using FitLog.Domain.Entities;
using MockQueryable.Moq;
using Microsoft.EntityFrameworkCore;
using Ardalis.GuardClauses;
using FitLog.Application.WorkoutTemplates.Commands.CreateWorkoutTemplate;
using FitLog.Application.Exercises.Queries.GetExercises;
using FitLog.Application.Use_Cases.WorkoutTemplates.Queries.GetWorkoutTemplateDetails;


namespace FitLog.Application.UnitTests.Use_Cases.Programs;
public class GetProgramDetailsQueryHandlerTests
{
    private readonly Mock<IApplicationDbContext> _mockContext;
    private readonly Mock<IMapper> _mockMapper;
    private readonly GetWorkoutProgramDetailsQueryHandler _handler;

    public GetProgramDetailsQueryHandlerTests()
    {
        _mockContext = new Mock<IApplicationDbContext>();
        _mockMapper = new Mock<IMapper>();
        _handler = new GetWorkoutProgramDetailsQueryHandler(_mockContext.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task Handle_GivenValidRequest_ReturnsProgramDetails()
    {
        // Arrange
        var programId = 1;
        var query = new GetWorkoutProgramDetailsQuery
        {
            Id = programId
        };

        var Program = new Program
        {
            ProgramId = programId,
            ProgramName = "Program1",
            User = new AspNetUser { Id = "user123", UserName = "user123", FirstName = "John", LastName = "Doe" },
            ProgramWorkouts = new List<ProgramWorkout>
                {
                    new ProgramWorkout
                    {
                        WorkoutTemplate = new WorkoutTemplate
                        {
                            TemplateName = "Workout1",
                            WorkoutTemplateExercises = new List<WorkoutTemplateExercise>
                            {
                                new WorkoutTemplateExercise
                                {
                                    ExerciseId = 1,
                                }
                            }
                        }
                    }
                }
        };
        var Exercise = new Exercise { ExerciseId = 1, ExerciseName = "Squat" };
        var Exercises = new List<Exercise> { Exercise }.AsQueryable().BuildMockDbSet();
        _mockContext.Setup(x => x.Exercises).Returns(Exercises.Object);

        var Programs = new List<Program> { Program }.AsQueryable().BuildMockDbSet();
        _mockContext.Setup(x => x.Programs).Returns(Programs.Object);

        var ProgramDetailsDto = new WorkoutProgramDetailsDto
        {
            ProgramId = programId,
            ProgramName = "Program1",
            UserId = "user123",
            UserName = "user123",
            CreatorFullName = "John Doe",
            ProgramWorkouts = new List<ProgramWorkoutDto>
                {
                    new ProgramWorkoutDto
                    {
                        WorkoutTemplate = new Application.WorkoutTemplates.Queries.GetWorkoutTemplateDetails.WorkoutTemplateDetailsDto(){
                            TemplateName = "Workout1",
                            WorkoutTemplateExercises = new List<WorkoutTemplateExerciseDTO>
                            {
                                new WorkoutTemplateExerciseDTO(){
                                    Exercise = new ExerciseDTO(){ ExerciseName = "Squat" }
                                }
                            }
                        }
                    }
                }
        };

        _mockMapper.Setup(m => m.Map<WorkoutProgramDetailsDto>(It.IsAny<Program>())).Returns(ProgramDetailsDto);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.ProgramId.Should().Be(programId);
        result.ProgramName.Should().Be("Program1");
        result.UserId.Should().Be("user123");
        result.UserName.Should().Be("user123");
        result.CreatorFullName.Should().Be("John Doe");
        result.ProgramWorkouts.Should().HaveCount(1);
        result.ProgramWorkouts.First().WorkoutTemplate?.TemplateName.Should().Be("Workout1");
        result.ProgramWorkouts.First().WorkoutTemplate?.WorkoutTemplateExercises.Should().HaveCount(1);
        result.ProgramWorkouts.First().WorkoutTemplate?.WorkoutTemplateExercises.First()?.Exercise?.ExerciseName.Should().Be("Squat");
    }

    [Fact]
    public async Task Handle_GivenNonExistentProgram_ThrowsNotFoundException()
    {
        // Arrange
        var programId = 2; // Non-existent ID
        var query = new GetWorkoutProgramDetailsQuery
        {
            Id = programId
        };

        var Programs = new List<Program>().AsQueryable().BuildMockDbSet();
        _mockContext.Setup(x => x.Programs).Returns(Programs.Object);

        // Act
        Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
            //.WithMessage($"Program not found")
            ;
    }

    [Fact]
    public async Task Handle_InvalidProgramId_ValidationReturnsFalse()
    {
        // Arrange
        var query = new GetWorkoutProgramDetailsQuery
        {
            Id = 0 // Invalid ID
        };

        var validator = new GetWorkoutProgramDetailsQueryValidator();
        var validationResult = await validator.ValidateAsync(query);

        // Assert
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Should().ContainSingle(e => e.PropertyName == "Id");
    }
}
