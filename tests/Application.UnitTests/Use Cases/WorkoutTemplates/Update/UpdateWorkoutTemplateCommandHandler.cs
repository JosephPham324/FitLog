using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using FitLog.Application.Common.Exceptions;
using FitLog.Application.Common.Interfaces;
using FitLog.Application.WorkoutTemplates.Commands.UpdateWorkoutTemplate;
using FitLog.Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace FitLog.Application.UnitTests.Use_Cases.WorkoutTemplates.Update
{
    public class UpdateWorkoutTemplateCommandHandlerTests
    {
        private readonly Mock<IApplicationDbContext> _contextMock;
        private readonly UpdateWorkoutTemplateCommandHandler _handler;

        public UpdateWorkoutTemplateCommandHandlerTests()
        {
            _contextMock = new Mock<IApplicationDbContext>();
            _handler = new UpdateWorkoutTemplateCommandHandler(_contextMock.Object);
        }

        [Fact]
        public async Task Handle_ValidCommand_ShouldUpdateWorkoutTemplate()
        {
            // Arrange
            var command = new UpdateWorkoutTemplateCommand
            {
                Id = 1, // Existing ID
                TemplateName = "Updated Template",
                Duration = "45 mins",
                IsPublic = true,
                WorkoutTemplateExercises = new List<WorkoutTemplateExerciseDto>
            {
                new WorkoutTemplateExerciseDto
                {
                    ExerciseId = 2,
                    OrderInSession = 1,
                    OrderInSuperset = 1,
                    Note = "Updated Note",
                    SetsRecommendation = 4,
                    IntensityPercentage = 80,
                    RpeRecommendation = 9,
                    WeightsUsed = "60kg",
                    NumbersOfReps = "12"
                }
            }
            };

            var existingTemplate = new WorkoutTemplate
            {
                Id = command.Id,
                TemplateName = "Initial Template",
                Duration = "30 mins",
                IsPublic = false,
                WorkoutTemplateExercises = new List<WorkoutTemplateExercise>
            {
                new WorkoutTemplateExercise
                {
                    ExerciseId = 1,
                    OrderInSession = 1,
                    OrderInSuperset = 1,
                    Note = "Initial Note",
                    SetsRecommendation = 3,
                    IntensityPercentage = 70,
                    RpeRecommendation = 8,
                    WeightsUsed = "50kg",
                    NumbersOfReps = "10"
                }
            }
            };

            _contextMock.Setup(m => m.WorkoutTemplates.FindAsync(It.IsAny<object[]>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(existingTemplate); // Simulate finding existing template

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeTrue(); // Ensure handle method returns true upon successful update

            // Verify that SaveChangesAsync was called once
            _contextMock.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

            // Verify the updated fields
            existingTemplate.TemplateName.Should().Be(command.TemplateName);
            existingTemplate.Duration.Should().Be(command.Duration);
            existingTemplate.IsPublic.Should().Be(command.IsPublic);
            existingTemplate.WorkoutTemplateExercises.Should().HaveCount(1);

            var updatedExercise = existingTemplate.WorkoutTemplateExercises.First();
            updatedExercise.ExerciseId.Should().Be(command.WorkoutTemplateExercises.First().ExerciseId);
            updatedExercise.OrderInSession.Should().Be(command.WorkoutTemplateExercises.First().OrderInSession);
            updatedExercise.OrderInSuperset.Should().Be(command.WorkoutTemplateExercises.First().OrderInSuperset);
            updatedExercise.Note.Should().Be(command.WorkoutTemplateExercises.First().Note);
            updatedExercise.SetsRecommendation.Should().Be(command.WorkoutTemplateExercises.First().SetsRecommendation);
            updatedExercise.IntensityPercentage.Should().Be(command.WorkoutTemplateExercises.First().IntensityPercentage);
            updatedExercise.RpeRecommendation.Should().Be(command.WorkoutTemplateExercises.First().RpeRecommendation);
            updatedExercise.WeightsUsed.Should().Be(command.WorkoutTemplateExercises.First().WeightsUsed);
            updatedExercise.NumbersOfReps.Should().Be(command.WorkoutTemplateExercises.First().NumbersOfReps);
        }

        [Fact]
        public async Task Handle_InvalidCommand_ShouldThrowNotFoundException()
        {
            // Arrange
            var command = new UpdateWorkoutTemplateCommand
            {
                Id = 999, // Non-existing ID
                TemplateName = "Updated Template",
                Duration = "45 mins",
                IsPublic = true,
                WorkoutTemplateExercises = new List<WorkoutTemplateExerciseDto>
            {
                new WorkoutTemplateExerciseDto
                {
                    ExerciseId = 2,
                    OrderInSession = 1,
                    OrderInSuperset = 1,
                    Note = "Updated Note",
                    SetsRecommendation = 4,
                    IntensityPercentage = 80,
                    RpeRecommendation = 9,
                    WeightsUsed = "60kg",
                    NumbersOfReps = "12"
                }
            }
            };

            _contextMock.Setup(m => m.WorkoutTemplates.FindAsync(It.IsAny<object[]>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync((WorkoutTemplate?)null); // Simulate not finding existing template

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }

}
