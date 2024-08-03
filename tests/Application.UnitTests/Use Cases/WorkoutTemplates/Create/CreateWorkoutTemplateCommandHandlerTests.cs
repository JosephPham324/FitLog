//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using FitLog.Application.Common.Interfaces;
//using FitLog.Application.WorkoutTemplates.Commands.CreateWorkoutTemplate;
//using FitLog.Domain.Entities;
//using Moq;
//using Xunit;

//namespace FitLog.Application.UnitTests.Use_Cases.WorkoutTemplates.Create;
//public class CreateWorkoutTemplateCommandHandlerTests
//{
//    private readonly Mock<IApplicationDbContext> _contextMock;
//    private readonly CreateWorkoutTemplateCommandHandler _handler;

//    public CreateWorkoutTemplateCommandHandlerTests()
//    {
//        _contextMock = new Mock<IApplicationDbContext>();
//        _handler = new CreateWorkoutTemplateCommandHandler(_contextMock.Object);
//    }

//    [Fact]
//    public async Task Handle_ShouldAddWorkoutTemplateToContext()
//    {
//        // Arrange
//        var command = new CreateWorkoutTemplateCommand
//        {
//            TemplateName = "Test Template",
//            Duration = "30 mins",
//            IsPublic = false,
//            WorkoutTemplateExercises = new List<WorkoutTemplateExerciseDto>
//                {
//                    new WorkoutTemplateExerciseDto
//                    {
//                        ExerciseId = 1,
//                        OrderInSession = 1,
//                        OrderInSuperset = 1,
//                        Note = "Note1",
//                        SetsRecommendation = 3,
//                        IntensityPercentage = 70,
//                        RpeRecommendation = 8,
//                        WeightsUsed = "50kg",
//                        NumbersOfReps = "10"
//                    }
//                }
//        };

//        var addedTemplate = null as WorkoutTemplate;

//        _contextMock.Setup(m => m.WorkoutTemplates.Add(It.IsAny<WorkoutTemplate>()))
//                    .Callback<WorkoutTemplate>(wt => addedTemplate = wt);

//        _contextMock.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

//        // Act
//        var result = await _handler.Handle(command, CancellationToken.None);

//        // Assert
//        _contextMock.Verify(m => m.WorkoutTemplates.Add(It.Is<WorkoutTemplate>(wt =>
//            wt.TemplateName == command.TemplateName &&
//            wt.Duration == command.Duration &&
//            wt.IsPublic == command.IsPublic &&
//            wt.WorkoutTemplateExercises.Count == command.WorkoutTemplateExercises.Count
//        )), Times.Once);

//        _contextMock.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

//        // Assert the result (ID of the added workout template)
//        Assert.NotNull(addedTemplate);
//        Assert.Equal(addedTemplate.Id, result); // Compare with ID of added workout template

//        // Additional Assert to ensure correct linking of WorkoutTemplateExercises
//        var exerciseList = new List<WorkoutTemplateExercise>(addedTemplate.WorkoutTemplateExercises);
//        Assert.Single(exerciseList);
//        var exercise = exerciseList[0];
//        var exerciseDto = ((List<WorkoutTemplateExerciseDto>)command.WorkoutTemplateExercises)[0];
//        Assert.Equal(exerciseDto.ExerciseId, exercise.ExerciseId);
//        Assert.Equal(exerciseDto.OrderInSession, exercise.OrderInSession);
//        Assert.Equal(exerciseDto.OrderInSuperset, exercise.OrderInSuperset);
//        Assert.Equal(exerciseDto.Note, exercise.Note);
//        Assert.Equal(exerciseDto.SetsRecommendation, exercise.SetsRecommendation);
//        Assert.Equal(exerciseDto.IntensityPercentage, exercise.IntensityPercentage);
//        Assert.Equal(exerciseDto.RpeRecommendation, exercise.RpeRecommendation);
//        Assert.Equal(exerciseDto.WeightsUsed, exercise.WeightsUsed);
//        Assert.Equal(exerciseDto.NumbersOfReps, exercise.NumbersOfReps);
//    }

//    // Add more tests for validation scenarios if needed
//}



