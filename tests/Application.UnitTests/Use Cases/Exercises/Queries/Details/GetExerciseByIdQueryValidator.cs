using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitLog.Application.Exercises.Queries.GetExerciseDetails;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace FitLog.Application.UnitTests.Use_Cases.Exercises.Queries.Details;
public class GetExerciseByIdQueryValidatorTests
{
    private GetExerciseByIdQueryValidator _validator;

    [SetUp]
    public void Setup()
    {
        _validator = new GetExerciseByIdQueryValidator();
    }

    [Test]
    public void Id_ShouldBeGreaterThanZero()
    {
        // Arrange
        var query = new GetExerciseByIdQuery { Id = 0 };

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id)
              .WithErrorMessage("ID must be greater than 0.");
    }

    [Test]
    public void ValidQuery_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var query = new GetExerciseByIdQuery { Id = 1 };

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}

