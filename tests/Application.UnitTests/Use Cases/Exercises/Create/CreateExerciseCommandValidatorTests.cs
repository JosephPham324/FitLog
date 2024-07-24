using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitLog.Application.Exercises.Commands.CreateExercise;
using FitLog.Domain.Entities;
using FitLog.Infrastructure.Data;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace FitLog.Application.UnitTests.Use_Cases.Exercises.Create;
public class CreateExerciseCommandValidatorTests
{
    private ApplicationDbContext _context;
    private CreateExerciseCommandValidator _validator;
    private List<MuscleGroup> _addedMuscleGroups;
    private List<Equipment> _addedEquipments;
    private List<Exercise> _addedExercises;
    private AspNetUser _user;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>();

        _context = new ApplicationDbContext();

        _addedMuscleGroups = new List<MuscleGroup>();
        _addedEquipments = new List<Equipment>();
        _addedExercises = new List<Exercise>();


        // Thiết lập dữ liệu test cho MuscleGroups và Equipment
        var muscleGroup = new MuscleGroup { MuscleGroupName = "Test Muscle Group" };
        var equipment = new Equipment { EquipmentName = "Test Equipment", ImageUrl = "http://example.com/image.jpg" };

        _context.MuscleGroups.Add(muscleGroup);
        _context.Equipment.Add(equipment);
        _context.SaveChanges();

        // Lưu thực thể được thêm vào danh sách
        _addedMuscleGroups.Add(muscleGroup);
        _addedEquipments.Add(equipment);

        _validator = new CreateExerciseCommandValidator(_context);

        _user = _context.AspNetUsers.FirstOrDefault() ?? new AspNetUser();
    }

    [TearDown]
    public void TearDown()
    {

        if (_addedMuscleGroups.Any())
        {
            _context.MuscleGroups.RemoveRange(_addedMuscleGroups);
        }

        if (_addedEquipments.Any())
        {
            _context.Equipment.RemoveRange(_addedEquipments);
        }

        if (_addedExercises.Any())
        {
            _context.Exercises.RemoveRange(_addedExercises);
        }

        _context.SaveChanges();
        _context.Dispose();
    }

    [Test]
    public async Task ValidCommand_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var muscleGroup = _addedMuscleGroups.First();
        var equipment = _addedEquipments.First();

        var command = new CreateExerciseCommand
        {
            CreatedBy = _user.Id,
            MuscleGroupIds = new List<int> { muscleGroup.MuscleGroupId },
            EquipmentId = equipment.EquipmentId,
            ExerciseName = "Test Exercise",
            DemoUrl = "http://example.com/demo",
            Type = "Weight Resistance",
            Description = "Test Description",
            PublicVisibility = true
        };

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public async Task CreatedBy_ShouldNotBeEmpty()
    {
        // Arrange
        var muscleGroup = _addedMuscleGroups.First();
        var equipment = _addedEquipments.First();

        var command = new CreateExerciseCommand
        {
            CreatedBy = "",
            MuscleGroupIds = new List<int> { muscleGroup.MuscleGroupId },
            EquipmentId = equipment.EquipmentId,
            ExerciseName = "Test Exercise",
            DemoUrl = "http://example.com/demo",
            Type = "WeightResistance",
            Description = "Test Description",
            PublicVisibility = true
        };

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CreatedBy)
              .WithErrorMessage("User does not exist.");
    }

    [Test]
    public async Task ExerciseName_ShouldNotBeEmpty()
    {
        // Arrange
        var muscleGroup = _addedMuscleGroups.First();
        var equipment = _addedEquipments.First();

        var command = new CreateExerciseCommand
        {
            CreatedBy = _user.Id,
            MuscleGroupIds = new List<int> { muscleGroup.MuscleGroupId },
            EquipmentId = equipment.EquipmentId,
            ExerciseName = "",
            DemoUrl = "http://example.com/demo",
            Type = "WeightResistance",
            Description = "Test Description",
            PublicVisibility = true
        };

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ExerciseName)
              .WithErrorMessage("'Exercise Name' must not be empty.");
    }

    [Test]
    public async Task DemoUrl_ShouldBeAValidUrl()
    {

        // Arrange
        var muscleGroup = _addedMuscleGroups.First();
        var equipment = _addedEquipments.First();

        var command = new CreateExerciseCommand
        {
            CreatedBy = _user.Id,
            MuscleGroupIds = new List<int> { muscleGroup.MuscleGroupId },
            EquipmentId = equipment.EquipmentId,
            ExerciseName = "Test Exercise",
            DemoUrl = "invalid-url",
            Type = "WeightResistance",
            Description = "Test Description",
            PublicVisibility = true
        };

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.DemoUrl)
              .WithErrorMessage("Invalid URL format.");
    }

    [Test]
    public async Task Type_ShouldBeAValidExerciseType()
    {
        var muscleGroup = _addedMuscleGroups.First();
        var equipment = _addedEquipments.First();

        var command = new CreateExerciseCommand
        {
            CreatedBy = _user.Id,
            MuscleGroupIds = new List<int> { muscleGroup.MuscleGroupId },
            EquipmentId = equipment.EquipmentId,
            ExerciseName = "Test Exercise",
            DemoUrl = "invalid-url",
            Type = "",
            Description = "Test Description",
            PublicVisibility = true
        };

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Type)
              .WithErrorMessage("Invalid exercise type.");
    }
}

