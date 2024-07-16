//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using FitLog.Application.MuscleGroups.Queries.GetMuscleGroupDetails;
//using FluentValidation.TestHelper;
//using NUnit.Framework;

//namespace FitLog.Application.UnitTests.Use_Cases.MuscleGroups.Queries.Details;
//public class GetMuscleGroupDetailsQueryValidatorTests
//{
//    private GetMuscleGroupDetailsQueryValidator _validator;

//    [SetUp]
//    public void Setup()
//    {
//        _validator = new GetMuscleGroupDetailsQueryValidator();
//    }

//    [Test]
//    public void Id_ShouldHaveValidationError_WhenIdIsZero()
//    {
//        // Arrange
//        var query = new GetMuscleGroupDetailsQuery { Id = 0 };

//        // Act
//        var result = _validator.TestValidate(query);

//        // Assert
//        result.ShouldHaveValidationErrorFor(x => x.Id)
//              .WithErrorMessage("Id must be specified.");
//    }

//    [Test]
//    public void Id_ShouldNotHaveValidationError_WhenIdIsPositive()
//    {
//        // Arrange
//        var query = new GetMuscleGroupDetailsQuery { Id = 1 };

//        // Act
//        var result = _validator.TestValidate(query);

//        // Assert
//        result.ShouldNotHaveValidationErrorFor(x => x.Id);
//    }
//}

