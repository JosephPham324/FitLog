using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitLog.Application.Equipments.Queries.GetEquipmentDetails;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace FitLog.Application.UnitTests.Use_Cases.Equipments.Queries.Details;
public class GetEquipmentDetailsQueryValidatorTests
{
    private GetEquipmentDetailsQueryValidator _validator;

    [SetUp]
    public void Setup()
    {
        _validator = new GetEquipmentDetailsQueryValidator();
    }



    [Test]
    public void ValidQuery_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var query = new GetEquipmentDetailsQuery { EquipmentId = 1 };

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void EquipmentId_ShouldBeGreaterThanZero()
    {
        // Arrange
        var query = new GetEquipmentDetailsQuery { EquipmentId = 0 };

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.EquipmentId)
              .WithErrorMessage("'Equipment Id' must be greater than '0'.");
    }
}

