using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitLog.Application.Equipments.Commands.UpdateEquipment;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace FitLog.Application.UnitTests.Use_Cases.Equipments.Update;
public class UpdateEquipmentCommandValidatorTests
{
    private UpdateEquipmentCommandValidator _validator;

    [SetUp]
    public void Setup()
    {
        _validator = new UpdateEquipmentCommandValidator();
    }

    [Test]
    public void EquipmentId_ShouldBeGreaterThanZero()
    {
        // Arrange
        var command = new UpdateEquipmentCommand { EquipmentId = 0, EquipmentName = "Test Equipment", ImageUrl = "http://example.com/image.jpg" };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.EquipmentId)
              .WithErrorMessage("'Equipment Id' must be greater than '0'.");
    }

    [Test]
    public void EquipmentName_ShouldNotBeEmpty()
    {
        // Arrange
        var command = new UpdateEquipmentCommand { EquipmentId = 1, EquipmentName = "", ImageUrl = "http://example.com/image.jpg" };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.EquipmentName)
              .WithErrorMessage("'Equipment Name' must not be empty.");
    }

    [Test]
    public void EquipmentName_ShouldHaveMaximumLength()
    {
        // Arrange
        var command = new UpdateEquipmentCommand { EquipmentId = 1, EquipmentName = new string('a', 201), ImageUrl = "http://example.com/image.jpg" };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.EquipmentName)
              .WithErrorMessage("The length of 'EquipmentName' must be 200 characters or fewer. You entered 201 characters.");
    }

    [Test]
    public void ImageUrl_ShouldBeValid()
    {
        // Arrange
        var command = new UpdateEquipmentCommand { EquipmentId = 1, EquipmentName = "Test Equipment", ImageUrl = "invalid-url" };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ImageUrl)
              .WithErrorMessage("Invalid URL format.");
    }

    [Test]
    public void ValidCommand_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = new UpdateEquipmentCommand { EquipmentId = 1, EquipmentName = "Valid Equipment", ImageUrl = "http://example.com/image.jpg" };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}

