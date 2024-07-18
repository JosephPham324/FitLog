using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitLog.Application.Equipments.Commands.Delete;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace FitLog.Application.UnitTests.Use_Cases.Equipments.Delete;
public class DeleteEquipmentCommandValidatorTests
{
    private DeleteEquipmentCommandValidator _validator;

    [SetUp]
    public void Setup()
    {
        _validator = new DeleteEquipmentCommandValidator();
    }

    [Test]
    public void ValidCommand_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = new DeleteEquipmentCommand { EquipmentId = 1 };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void EquipmentId_ShouldBeGreaterThanZero()
    {
        // Arrange
        var command = new DeleteEquipmentCommand { EquipmentId = 0 };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.EquipmentId)
              .WithErrorMessage("'Equipment Id' must be greater than '0'.");
    }
}

