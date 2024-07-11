using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitLog.Application.MuscleGroups.Queries.GetMuscleGroupsListWithPagination;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace FitLog.Application.UnitTests.Use_Cases.MuscleGroups.Queries.GetAll;
public class GetMuscleGroupsListWithPaginationQueryValidatorTests
{
    private GetMuscleGroupsListWithPaginationQueryValidator _validator;

    [SetUp]
    public void Setup()
    {
        _validator = new GetMuscleGroupsListWithPaginationQueryValidator();
    }

    [Test]
    public void ValidQuery_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var query = new GetMuscleGroupsListWithPaginationQuery { PageNumber = 1, PageSize = 10 };

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void PageNumber_ShouldBeAtLeastOne()
    {
        // Arrange
        var query = new GetMuscleGroupsListWithPaginationQuery { PageNumber = 0, PageSize = 10 };

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PageNumber)
              .WithErrorMessage("Page number must be at least 1.");
    }

    [Test]
    public void PageSize_ShouldBeAtLeastOne()
    {
        // Arrange
        var query = new GetMuscleGroupsListWithPaginationQuery { PageNumber = 1, PageSize = 0 };

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PageSize)
              .WithErrorMessage("Page size must be at least 1.");
    }
}

