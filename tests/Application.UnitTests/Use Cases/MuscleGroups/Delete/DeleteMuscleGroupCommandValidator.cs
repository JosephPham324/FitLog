using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitLog.Application.MuscleGroups.Commands.DeleteMuscleGroup;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace FitLog.Application.UnitTests.Use_Cases.MuscleGroups.Delete;
public class DeleteMuscleGroupCommandValidatorTests
{
    private DeleteMuscleGroupCommandValidator _validator;

    [SetUp]
    public void Setup()
    {
        _validator = new DeleteMuscleGroupCommandValidator();
    }

    [Test]
    public void Id_ShouldHaveValidationError_WhenIdIsZero()
    {
        // Arrange
        var command = new DeleteMuscleGroupCommand { Id = 0 };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id)
              .WithErrorMessage("Id must be greater than zero.");
    }

    [Test]
    public void Id_ShouldHaveValidationError_WhenIdIsNegative()
    {
        // Arrange
        var command = new DeleteMuscleGroupCommand { Id = -1 };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id)
              .WithErrorMessage("Id must be greater than zero.");
    }

    [Test]
    public void Id_ShouldNotHaveValidationError_WhenIdIsPositive()
    {
        // Arrange
        var command = new DeleteMuscleGroupCommand { Id = 1 };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }
}

