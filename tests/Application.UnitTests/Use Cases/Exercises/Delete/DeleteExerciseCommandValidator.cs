using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitLog.Application.Exercises.Commands.DeleteExercise;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace FitLog.Application.UnitTests.Use_Cases.Exercises.Delete;
public class DeleteExerciseCommandValidatorTests
{
    private DeleteExerciseCommandValidator _validator;

    [SetUp]
    public void Setup()
    {
        _validator = new DeleteExerciseCommandValidator();
    }

    [Test]
    public void ExerciseId_ShouldBeGreaterThanZero()
    {
        // Arrange
        var command = new DeleteExerciseCommand
        {
            ExerciseId = 0
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ExerciseId)
              .WithErrorMessage("'Exercise Id' must be greater than '0'.");
    }

}

