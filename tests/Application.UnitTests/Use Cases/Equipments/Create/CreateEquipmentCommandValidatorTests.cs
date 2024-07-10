using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FitLog.Application.Common.Interfaces;
using FitLog.Application.Equipments.Commands.CreateEquipment;
using FitLog.Domain.Entities;
using FitLog.Infrastructure.Data;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace FitLog.Application.UnitTests.Use_Cases.Equipments.Create;
public class CreateEquipmentCommandValidatorTests
{
    private CreateEquipmentCommandValidator _validator;
    private ApplicationDbContext _context;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;

        _context = new ApplicationDbContext(options);
        _validator = new CreateEquipmentCommandValidator(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }

    [Test]
    public async Task EquipmentName_ShouldNotBeEmpty()
    {
        // Arrange
        var command = new CreateEquipmentCommand { EquipmentName = "", ImageUrl = "http://example.com/image.jpg" };

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.EquipmentName)
              .WithErrorMessage("'Equipment Name' must not be empty.");
    }


    [Test]
    public async Task EquipmentName_ShouldHaveMaximumLength()
    {
        // Arrange
        var command = new CreateEquipmentCommand { EquipmentName = new string('a', 201), ImageUrl = "http://example.com/image.jpg" };

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.EquipmentName)
              .WithErrorMessage("The length of 'Equipment Name' must be 200 characters or fewer. You entered 201 characters.");
    }


    [Test]
    public async Task ImageUrl_ShouldNotBeEmpty()
    {
        // Arrange
        var command = new CreateEquipmentCommand { EquipmentName = "Test Equipment", ImageUrl = "" };

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ImageUrl)
              .WithErrorMessage("Image URL cannot be empty.");
    }


    [Test]
    public async Task ImageUrl_ShouldBeValid()
    {
        // Arrange
        var command = new CreateEquipmentCommand { EquipmentName = "Test Equipment", ImageUrl = "invalid-url" };

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ImageUrl)
              .WithErrorMessage("Invalid URL format.");
    }


    [Test]
    public async Task EquipmentName_ShouldBeUnique()
    {
        // Arrange
        // Add an existing Equipment directly to the context
        var existingEquipment = new Equipment { EquipmentName = "Existing Equipment" };
        await _context.Equipment.AddAsync(existingEquipment);
        await _context.SaveChangesAsync();

        var command = new CreateEquipmentCommand { EquipmentName = "Existing Equipment", ImageUrl = "http://example.com/image.jpg" };

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.EquipmentName)
              .WithErrorMessage("'Equipment Name' already exists!");
    }


    [Test]
    public async Task ValidCommand_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = new CreateEquipmentCommand { EquipmentName = "Valid Equipment", ImageUrl = "http://example.com/image.jpg" };

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }


}


