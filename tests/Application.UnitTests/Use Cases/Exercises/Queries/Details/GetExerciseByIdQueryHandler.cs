using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Models;
using FitLog.Application.Exercises.Queries.GetExerciseDetails;
using FitLog.Domain.Entities;
using Moq;
using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Ardalis.GuardClauses;

namespace FitLog.Application.UnitTests.Use_Cases.Exercises.Queries.Details
{
    public class GetExerciseByIdQueryHandlerTests
    {
        private readonly Mock<IApplicationDbContext> _contextMock;
        private readonly IMapper _mapper;
        private readonly GetExerciseByIdQueryHandler _handler;

        public GetExerciseByIdQueryHandlerTests()
        {
            _contextMock = new Mock<IApplicationDbContext>();

            var muscleGroupsDbSetMock = new List<MuscleGroup>().AsQueryable().BuildMockDbSet();
            var equipmentDbSetMock = new List<Equipment>().AsQueryable().BuildMockDbSet();
            var exercisesDbSetMock = new List<Exercise>().AsQueryable().BuildMockDbSet();
            var usersDbSetMock = new List<AspNetUser>().AsQueryable().BuildMockDbSet();

            _contextMock.Setup(x => x.MuscleGroups).Returns(muscleGroupsDbSetMock.Object);
            _contextMock.Setup(x => x.Equipment).Returns(equipmentDbSetMock.Object);
            _contextMock.Setup(x => x.Exercises).Returns(exercisesDbSetMock.Object);
            _contextMock.Setup(x => x.AspNetUsers).Returns(usersDbSetMock.Object);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ExerciseDetailsDTO.Mapping());
            });
            _mapper = config.CreateMapper();

            _handler = new GetExerciseByIdQueryHandler(_contextMock.Object, _mapper);
        }

        [Fact]
        public async Task Handle_ValidId_ReturnsExerciseDetailsDTO()
        {
            // Arrange
            var user = new AspNetUser
            {
                Id = "48fd07f4-2a6a-46ec-a577-db456fac44ce",
                UserName = "administrator@localhost",
                Email = "administrator@localhost"
            };

            var equipment = new Equipment
            {
                EquipmentId = 1,
                EquipmentName = "Test Equipment",
                ImageUrl = "http://example.com/image.jpg"
            };

            var exercise = new Exercise
            {
                ExerciseId = 1,
                CreatedBy = user.Id,
                EquipmentId = equipment.EquipmentId,
                ExerciseName = "Test Exercise",
                DemoUrl = "http://example.com/demo",
                Type = "WeightResistance",
                Description = "Test Description",
                PublicVisibility = true
            };

            var exercises = new List<Exercise> { exercise }.AsQueryable().BuildMockDbSet();
            var users = new List<AspNetUser> { user }.AsQueryable().BuildMockDbSet();
            var equipmentList = new List<Equipment> { equipment }.AsQueryable().BuildMockDbSet();

            _contextMock.Setup(x => x.Exercises).Returns(exercises.Object);
            _contextMock.Setup(x => x.AspNetUsers).Returns(users.Object);
            _contextMock.Setup(x => x.Equipment).Returns(equipmentList.Object);

            var query = new GetExerciseByIdQuery { Id = exercise.ExerciseId };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.ExerciseId.Should().Be(exercise.ExerciseId);
            result.CreatedByName.Should().Be(user.UserName);
            result.EquipmentName.Should().Be(equipment.EquipmentName);
        }

        [Fact]
        public void Handle_InvalidId_ThrowsNotFoundException()
        {
            // Arrange
            var query = new GetExerciseByIdQuery { Id = 99 };

            // Act
            Func<Task> act = async () => { await _handler.Handle(query, CancellationToken.None); };

            // Assert
            act.Should().ThrowAsync<NotFoundException>().WithMessage("Exercise (99) was not found.");
        }
    }
}
