using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitLog.Application.Exercises.Queries.GetExercsieTypes;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace FitLog.Application.UnitTests.Use_Cases.Exercises.Queries.GetTypes;
public class GetExercsieTypesQueryValidatorTests
{
    private GetExercsieTypesQueryValidator _validator;

    [SetUp]
    public void Setup()
    {
        _validator = new GetExercsieTypesQueryValidator();
    }

    [Test]
    public void Validate_ShouldNotHaveAnyValidationErrors()
    {
        // Arrange
        var query = new GetExercsieTypesQuery();

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}


