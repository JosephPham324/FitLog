////using System;
////using System.Collections.Generic;
////using System.Linq;
////using System.Text;
////using System.Threading.Tasks;
////using FitLog.Application.Common.Interfaces;
////using FitLog.Application.WorkoutTemplates.Commands.CreatePersonalTemplate;
////using FitLog.Domain.Entities;
////using Moq;
////using Xunit;

////namespace FitLog.Application.UnitTests.Use_Cases.WorkoutTemplates.Create;
////public class CreatePersonalTemplateCommandHandlerTests
////{
////    private readonly Mock<IApplicationDbContext> _contextMock;
////    private readonly Mock<IUserTokenService> _userTokenServiceMock;
////    private readonly CreatePersonalTemplateCommandHandler _handler;

////    public CreatePersonalTemplateCommandHandlerTests()
////    {
////        _contextMock = new Mock<IApplicationDbContext>();
////        _userTokenServiceMock = new Mock<IUserTokenService>();
////        _handler = new CreatePersonalTemplateCommandHandler(_contextMock.Object, _userTokenServiceMock.Object);
////    }

////    [Fact]
////    public async Task Handle_ShouldAddWorkoutTemplateToContext()
////    {
////        // Arrange
////        var command = new CreatePersonalTemplateCommand
////        {
////            UserToken = "testUserId",
////            TemplateName = "Test Template",
////            Duration = "30 mins",
////            WorkoutTemplateExercises = new List<PersonalTemplateExerciseDto>
////                {
////                    new PersonalTemplateExerciseDto
////                    {
////                        ExerciseId = 1,
////                        OrderInSession = 1,
////                        OrderInSuperset = 1,
////                        Note = "Note1",
////                        SetsRecommendation = 3,
////                        IntensityPercentage = 70,
////                        RpeRecommendation = 8,
////                        WeightsUsed = "50kg",
////                        NumbersOfReps = "10"
////                    }
////                }
////        };

////        var addedTemplate = null as WorkoutTemplate;

////        _contextMock.Setup(m => m.WorkoutTemplates.Add(It.IsAny<WorkoutTemplate>()))
////                    .Callback<WorkoutTemplate>(wt =>
////                    {
////                        // Convert PersonalTemplateExerciseDto to WorkoutTemplateExercise
////                        foreach (var exerciseDto in command.WorkoutTemplateExercises)
////                        {
////                            var workoutTemplateExercise = new WorkoutTemplateExercise
////                            {
////                                ExerciseId = exerciseDto.ExerciseId,
////                                OrderInSession = exerciseDto.OrderInSession,
////                                OrderInSuperset = exerciseDto.OrderInSuperset,
////                                Note = exerciseDto.Note,
////                                SetsRecommendation = exerciseDto.SetsRecommendation,
////                                IntensityPercentage = exerciseDto.IntensityPercentage,
////                                RpeRecommendation = exerciseDto.RpeRecommendation,
////                                WeightsUsed = exerciseDto.WeightsUsed,
////                                NumbersOfReps = exerciseDto.NumbersOfReps
////                            };

////                            wt.WorkoutTemplateExercises.Add(workoutTemplateExercise);
////                        }

////                        addedTemplate = wt;
////                    });

////        _contextMock.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()))
////                    .ReturnsAsync(1);

////        // Act
////        var result = await _handler.Handle(command, CancellationToken.None);

////        // Assert
////        _contextMock.Verify(m => m.WorkoutTemplates.Add(It.Is<WorkoutTemplate>(wt =>
////            wt.TemplateName == command.TemplateName &&
////            wt.Duration == command.Duration &&
////            wt.CreatedBy == command.UserToken &&
////            !wt.IsPublic &&
////            wt.WorkoutTemplateExercises.Count == command.WorkoutTemplateExercises.Count
////        )), Times.Once);

////        _contextMock.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

////        Assert.NotNull(addedTemplate);
////        Assert.Equal(addedTemplate.Id, result); // Ensure SaveChangesAsync returns 1

////        // Additional Assert to ensure correct linking of WorkoutTemplateExercises
////        Assert.NotNull(addedTemplate);
////        var exerciseList = new List<WorkoutTemplateExercise>(addedTemplate.WorkoutTemplateExercises);
////        Assert.Single(exerciseList);
////        var exercise = exerciseList[0];
////        var exerciseDto = command.WorkoutTemplateExercises.First(); // No need for casting here
////        Assert.Equal(exerciseDto.ExerciseId, exercise.ExerciseId);
////        Assert.Equal(exerciseDto.OrderInSession, exercise.OrderInSession);
////        Assert.Equal(exerciseDto.OrderInSuperset, exercise.OrderInSuperset);
////        Assert.Equal(exerciseDto.Note, exercise.Note);
////        Assert.Equal(exerciseDto.SetsRecommendation, exercise.SetsRecommendation);
////        Assert.Equal(exerciseDto.IntensityPercentage, exercise.IntensityPercentage);
////        Assert.Equal(exerciseDto.RpeRecommendation, exercise.RpeRecommendation);
////        Assert.Equal(exerciseDto.WeightsUsed, exercise.WeightsUsed);
////        Assert.Equal(exerciseDto.NumbersOfReps, exercise.NumbersOfReps);
////    }
////}