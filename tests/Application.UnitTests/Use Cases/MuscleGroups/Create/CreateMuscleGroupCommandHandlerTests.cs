using FitLog.Application.Common.Interfaces;
using FitLog.Application.MuscleGroups.Commands.CreateMuscleGroup;
using FitLog.Domain.Entities;
using FluentAssertions;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace FitLog.Application.UnitTests.Use_Cases.MuscleGroups.Create
{
    public class CreateMuscleGroupCommandHandlerTests
    {
        private readonly CreateMuscleGroupCommandHandler _handler;
        private readonly CreateMuscleGroupCommandValidator _validator;
        private readonly Mock<IApplicationDbContext> _mockDbContext;

        public CreateMuscleGroupCommandHandlerTests()
        {
            _mockDbContext = new Mock<IApplicationDbContext>();
            _mockDbContext.Setup(db => db.MuscleGroups).Returns(MockDbSet(new List<MuscleGroup>()).Object);

            _handler = new CreateMuscleGroupCommandHandler(_mockDbContext.Object);
            _validator = new CreateMuscleGroupCommandValidator(_mockDbContext.Object);
        }

        [Fact]
        public async Task Should_Have_Error_When_MuscleGroupName_Is_Empty()
        {
            var command = new CreateMuscleGroupCommand { MuscleGroupName = "" };
            var result = await _validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(mg => mg.MuscleGroupName);
        }

        [Fact]
        public async Task Should_Have_Error_When_MuscleGroupName_Is_Not_Unique()
        {
            var existingGroups = new List<MuscleGroup> { new MuscleGroup { MuscleGroupName = "ExistingName" } };
            _mockDbContext.Setup(m => m.MuscleGroups).Returns(MockDbSet(existingGroups).Object);

            var command = new CreateMuscleGroupCommand { MuscleGroupName = "ExistingName" };
            var result = await _validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(mg => mg.MuscleGroupName).WithErrorMessage("Muscle group name already exists.");
        }

        [Fact]
        public async Task Should_Have_Error_When_ImageUrl_Is_Invalid()
        {
            var command = new CreateMuscleGroupCommand { ImageUrl = "invalidurl" };
            var result = await _validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(mg => mg.ImageUrl).WithErrorMessage("Invalid URL format.");
        }

        [Fact]
        public async Task Should_Not_Have_Error_When_Inputs_Are_Valid()
        {
            var command = new CreateMuscleGroupCommand { MuscleGroupName = "NewName", ImageUrl = "http://validurl.com" };
            var result = await _validator.TestValidateAsync(command);
            result.ShouldNotHaveValidationErrorFor(mg => mg.MuscleGroupName);
            result.ShouldNotHaveValidationErrorFor(mg => mg.ImageUrl);
        }

        [Fact]
        public async Task Handle_Should_Add_MuscleGroup_To_Context()
        {
            // Arrange
            var command = new CreateMuscleGroupCommand
            {
                MuscleGroupName = "NewName",
                ImageUrl = "http://validurl.com"
            };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockDbContext.Verify(m => m.MuscleGroups.Add(It.Is<MuscleGroup>(mg => mg.MuscleGroupName == command.MuscleGroupName && mg.ImageUrl == command.ImageUrl)), Times.Once());
            _mockDbContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
            result.Success.Should().BeTrue();
        }

        private static Mock<DbSet<T>> MockDbSet<T>(List<T> list) where T : class
        {
            var queryable = list.AsQueryable();
            var dbSet = new Mock<DbSet<T>>();
            dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());
            dbSet.Setup(m => m.Add(It.IsAny<T>())).Callback<T>(list.Add);
            return dbSet;
        }
    }
}
