using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitLog.Application.MuscleGroups.Commands.UpdateMuscleGroup;
using FitLog.Infrastructure.Data;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace FitLog.Application.UnitTests.Use_Cases.MuscleGroups.Update;
public class UpdateMuscleGroupCommandValidatorTests
{
    private UpdateMuscleGroupCommandValidator _validator;
    private ApplicationDbContext _context;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .Options;
        _context = new ApplicationDbContext(options);
        _validator = new UpdateMuscleGroupCommandValidator(_context); // Pass null since context isn't needed for validator tests
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }

    [Test]
    public async Task Id_ShouldHaveValidationError_WhenIdIsZero()
    {
        // Arrange
        var command = new UpdateMuscleGroupCommand { Id = 0 };

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id)
              .WithErrorMessage("'Id' must not be empty.");
    }

    [Test]
    public async Task Id_ShouldNotHaveValidationError_WhenIdIsPositive()
    {
        // Arrange
        var command = new UpdateMuscleGroupCommand { Id = -1 };

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }

    [Test]
    public async Task MuscleGroupName_ShouldHaveValidationError_WhenMuscleGroupNameIsEmpty()
    {
        // Arrange
        var command = new UpdateMuscleGroupCommand { MuscleGroupName = "" };

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.MuscleGroupName)
              .WithErrorMessage("'Muscle Group Name' must not be empty.");
    }

    [Test]
    public async Task ImageUrl_ShouldHaveValidationError_WhenImageUrlIsInvalid()
    {
        // Arrange
        var command = new UpdateMuscleGroupCommand { ImageUrl = "" };

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ImageUrl)
              .WithErrorMessage("Invalid URL format.");
    }
}



