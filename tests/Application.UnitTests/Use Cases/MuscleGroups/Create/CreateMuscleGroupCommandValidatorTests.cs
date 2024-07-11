using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitLog.Application.Common.Interfaces;
using FitLog.Application.MuscleGroups.Commands.CreateMuscleGroup;
using FitLog.Domain.Entities;
using FitLog.Infrastructure.Data;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace FitLog.Application.UnitTests.Use_Cases.MuscleGroups.Create;
public class CreateMuscleGroupCommandValidatorTests
{
    private ApplicationDbContext _context;
    private CreateMuscleGroupCommandValidator _validator;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .Options;

        _context = new ApplicationDbContext(options);
        _validator = new CreateMuscleGroupCommandValidator(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }

    [Test]
    public async Task ValidCommand_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = new CreateMuscleGroupCommand
        {
            MuscleGroupName = "Legs",
            ImageUrl = "http://example.com/image"
        };

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public async Task MuscleGroupName_Empty_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateMuscleGroupCommand
        {
            MuscleGroupName = "",
            ImageUrl = "http://example.com/image"
        };

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.MuscleGroupName)
              .WithErrorMessage("'Muscle Group Name' must not be empty.");
    }

    [Test]
    public async Task ImageUrl_InvalidFormat_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateMuscleGroupCommand
        {
            MuscleGroupName = "Legs",
            ImageUrl = "invalid-url"
        };

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ImageUrl)
              .WithErrorMessage("Invalid URL format.");
    }
}



