using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FitLog.Application.Common.Interfaces;
using FitLog.Application.Exercises.Commands.CreateExercise;
using FitLog.Application.Exercises.Commands.DeleteExercise;
using FitLog.Application.Common.Models;
using FitLog.Domain.Entities;
using Moq;
using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;

namespace FitLog.Application.UnitTests.Use_Cases.Exercises.Commands
{
    public class DeleteExerciseCommandHandlerTests
    {
        private readonly Mock<IApplicationDbContext> _contextMock;
        private readonly DeleteExerciseCommandHandler _handler;
        private readonly CreateExerciseCommandHandler _handlerCreate;

        public DeleteExerciseCommandHandlerTests()
        {
            var user = new AspNetUser
            {
                Id = "user_id",
                UserName = "TestUser",
                Email = "testuser@example.com"
            };

            var muscleGroup = new MuscleGroup { MuscleGroupId = 1, MuscleGroupName = "Test Muscle Group", ImageUrl = "http://example.com/image.jpg" };
            var equipment = new Equipment { EquipmentId = 1, EquipmentName = "Test Equipment", ImageUrl = "http://example.com/image.jpg" };

            _contextMock = new Mock<IApplicationDbContext>();
            var exercise = new Exercise
            {
                ExerciseId = 1,
                CreatedBy = user.Id,
                ExerciseName = "Test Exercise",
                EquipmentId = equipment.EquipmentId,
                DemoUrl = "http://example.com/demo",
                Type = "WeightResistance",
                Description = "Test Description",
                PublicVisibility = true,
                ExerciseMuscleGroups = new List<ExerciseMuscleGroup> { new ExerciseMuscleGroup { MuscleGroupId = muscleGroup.MuscleGroupId } }
            };

            var exercisesDbSetMock = new List<Exercise>() { exercise }.AsQueryable().BuildMockDbSet();
            var muscleGroupsDbSetMock = new List<MuscleGroup>() { muscleGroup }.AsQueryable().BuildMockDbSet();
            var equipmentDbSetMock = new List<Equipment>() { equipment }.AsQueryable().BuildMockDbSet();
            var usersDbSetMock = new List<AspNetUser>().AsQueryable().BuildMockDbSet();

            _contextMock.Setup(x => x.Exercises).Returns(exercisesDbSetMock.Object);
            _contextMock.Setup(x => x.MuscleGroups).Returns(muscleGroupsDbSetMock.Object);
            _contextMock.Setup(x => x.Equipment).Returns(equipmentDbSetMock.Object);
            _contextMock.Setup(x => x.AspNetUsers).Returns(usersDbSetMock.Object);

            _handler = new DeleteExerciseCommandHandler(_contextMock.Object);
            _handlerCreate = new CreateExerciseCommandHandler(_contextMock.Object);
        }

        [Fact]
        public async Task Handle_ValidCommand_ReturnsTrue()
        {
            
            var command = new DeleteExerciseCommand
            {
                ExerciseId = 1
            };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Success.Should().BeTrue();
            _contextMock.Verify(x => x.Exercises.Remove(It.IsAny<Exercise>()), Times.Once);
            _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_InvalidExerciseId_ReturnsFalse()
        {
            // Arrange
            var command = new DeleteExerciseCommand
            {
                ExerciseId = 0
            };

            var exercises = new List<Exercise>().AsQueryable().BuildMockDbSet();
            _contextMock.Setup(x => x.Exercises).Returns(exercises.Object);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Success.Should().BeFalse();
            result.Errors.Should().Contain("Exercise not found");
        }

        [Fact]
        public async Task Handle_ExerciseNotInDatabase_ReturnsFalse()
        {
            // Arrange
            var command = new DeleteExerciseCommand
            {
                ExerciseId = 999
            };

            var exercises = new List<Exercise>().AsQueryable().BuildMockDbSet();
            _contextMock.Setup(x => x.Exercises).Returns(exercises.Object);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Success.Should().BeFalse();
            result.Errors.Should().Contain("Exercise not found");
        }
    }
}
