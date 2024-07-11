//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using FitLog.Application.Exercises.Commands.UpdateExercise;
//using FitLog.Infrastructure.Data;
//using FluentValidation.TestHelper;
//using Microsoft.EntityFrameworkCore;
//using NUnit.Framework;
//using NUnit.Framework.Internal;

//namespace FitLog.Application.UnitTests.Use_Cases.Exercises.Update;
//public class UpdateExerciseCommandValidatorTests
//{
//    private ApplicationDbContext _context;
//    private UpdateExerciseCommandValidator _validator;
//    private Domain.Entities.AspNetUser _testUser;
//    private Domain.Entities.MuscleGroup _testMuscleGroup;
//    private Domain.Entities.Equipment _testEquipment;
//    private Domain.Entities.Exercise _testExercise;

//    [SetUp]
//    public void Setup()
//    {
//        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
//            .Options;

//        _context = new ApplicationDbContext(options);

//        // Add test data
//        _testUser = new Domain.Entities.AspNetUser { UserName = "Test", PasswordHash = "123456" };
//        _testMuscleGroup = new Domain.Entities.MuscleGroup { MuscleGroupName = "Test" };
//        _testEquipment = new Domain.Entities.Equipment { EquipmentName = "Test" };
//        _testExercise = new Domain.Entities.Exercise
//        {
//            CreatedBy = "Test",
//            MuscleGroupId = _testMuscleGroup.MuscleGroupId,
//            EquipmentId = _testEquipment.EquipmentId,
//            ExerciseName = "Test Exercise",
//            DemoUrl = "http://example.com/demo",
//            Type = "WeightResistance",
//            Description = "Test Description",
//            PublicVisibility = true
//        };

//        _context.AspNetUsers.Add(_testUser);
//        _context.MuscleGroups.Add(_testMuscleGroup);
//        _context.Equipment.Add(_testEquipment);
//        _context.Exercises.Add(_testExercise);

//        _context.SaveChanges();

//        _validator = new UpdateExerciseCommandValidator(_context);
//    }

//    [TearDown]
//    public void TearDown()
//    {
//        // Clean up the database after each test
//        _context.Exercises.Remove(_testExercise);
//        _context.MuscleGroups.Remove(_testMuscleGroup);
//        _context.Equipment.Remove(_testEquipment);
//        //_context.AspNetUsers.Remove(_testUser);
//        _context.SaveChanges();

//        _context.Dispose();
//    }

//    [Test]
//    public async Task ValidCommand_ShouldNotHaveValidationErrors()
//    {
//        // Arrange
//        var command = new UpdateExerciseCommand
//        {
//            ExerciseId = _testExercise.ExerciseId,
//            CreatedBy = _testUser!.UserName,
//            MuscleGroupId = _testMuscleGroup!.MuscleGroupId,
//            EquipmentId = _testEquipment!.EquipmentId,
//            ExerciseName = "Updated Exercise",
//            DemoUrl = "http://example.com/demo",
//            Type = "WeightResistance",
//            Description = "Updated Description",
//            PublicVisibility = true
//        };

//        // Act
//        var result = await _validator.TestValidateAsync(command);

//        // Assert
//        result.ShouldNotHaveAnyValidationErrors();
//    }

//    [Test]
//    public async Task CreatedBy_ShouldNotBeEmpty()
//    {

//        // Arrange
//        var command = new UpdateExerciseCommand
//        {
//            ExerciseId = _testExercise.ExerciseId,
//            CreatedBy = "",
//            MuscleGroupId = _testMuscleGroup!.MuscleGroupId,
//            EquipmentId = _testEquipment!.EquipmentId,
//            ExerciseName = "Updated Exercise",
//            DemoUrl = "http://example.com/demo",
//            Type = "WeightResistance",
//            Description = "Updated Description",
//            PublicVisibility = true
//        };

//        // Act
//        var result = await _validator.TestValidateAsync(command);

//        // Assert
//        result.ShouldHaveValidationErrorFor(x => x.CreatedBy)
//              .WithErrorMessage("User does not exist.");
//    }

//    [Test]
//    public async Task ExerciseName_ShouldNotBeEmpty()
//    {

//        // Arrange
//        var command = new UpdateExerciseCommand
//        {
//            ExerciseId = _testExercise.ExerciseId,
//            CreatedBy = _testUser!.UserName,
//            MuscleGroupId = _testMuscleGroup!.MuscleGroupId,
//            EquipmentId = _testEquipment!.EquipmentId,
//            ExerciseName = "",
//            DemoUrl = "http://example.com/demo",
//            Type = "WeightResistance",
//            Description = "Updated Description",
//            PublicVisibility = true
//        };

//        // Act
//        var result = await _validator.TestValidateAsync(command);

//        // Assert
//        result.ShouldHaveValidationErrorFor(x => x.ExerciseName)
//              .WithErrorMessage("'Exercise Name' must not be empty.");
//    }

//    [Test]
//    public async Task DemoUrl_ShouldBeAValidUrl()
//    {
//        // Arrange
//        var command = new UpdateExerciseCommand
//        {
//            ExerciseId = _testExercise.ExerciseId,
//            CreatedBy = _testUser!.UserName,
//            MuscleGroupId = _testMuscleGroup!.MuscleGroupId,
//            EquipmentId = _testEquipment!.EquipmentId,
//            ExerciseName = "Updated Exercise",
//            DemoUrl = "",
//            Type = "WeightResistance",
//            Description = "Updated Description",
//            PublicVisibility = true
//        };

//        // Act
//        var result = await _validator.TestValidateAsync(command);

//        // Assert
//        result.ShouldHaveValidationErrorFor(x => x.DemoUrl)
//              .WithErrorMessage("Invalid URL format.");
//    }

//    [Test]
//    public async Task Type_ShouldBeAValidExerciseType()
//    {
//        // Arrange
//        var command = new UpdateExerciseCommand
//        {
//            ExerciseId = _testExercise.ExerciseId,
//            CreatedBy = _testUser!.UserName,
//            MuscleGroupId = _testMuscleGroup!.MuscleGroupId,
//            EquipmentId = _testEquipment!.EquipmentId,
//            ExerciseName = "Updated Exercise",
//            DemoUrl = "http://example.com/demo",
//            Type = "",
//            Description = "Updated Description",
//            PublicVisibility = true
//        };

//        // Act
//        var result = await _validator.TestValidateAsync(command);

//        // Assert
//        result.ShouldHaveValidationErrorFor(x => x.Type)
//              .WithErrorMessage("Invalid exercise type.");
//    }
//}

