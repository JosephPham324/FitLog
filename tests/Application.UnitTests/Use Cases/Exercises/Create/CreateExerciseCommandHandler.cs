using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitLog.Application.Exercises.Commands.CreateExercise;
using FitLog.Domain.Entities;
using FitLog.Infrastructure.Data;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace FitLog.Application.UnitTests.Use_Cases.Exercises.Create;
public class CreateExerciseCommandHandlerTests
{
    private ApplicationDbContext _context;
    private CreateExerciseCommandHandler _handler;
    private List<MuscleGroup> _addedMuscleGroups;
    private List<Equipment> _addedEquipments;
    private List<Exercise> _addedExercises;
    private AspNetUser _user;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .Options;

        _context = new ApplicationDbContext(options);
        _handler = new CreateExerciseCommandHandler(_context);

        // Thiết lập danh sách để lưu thực thể được thêm vào
        _addedMuscleGroups = new List<MuscleGroup>();
        _addedEquipments = new List<Equipment>();
        _addedExercises = new List<Exercise>();
        

        // Thiết lập dữ liệu test cho MuscleGroups và Equipment
        var muscleGroup = new MuscleGroup { MuscleGroupName = "Test Muscle Group" };
        var equipment = new Equipment { EquipmentName = "Test Equipment", ImageUrl = "http://example.com/image.jpg" };

        _context.MuscleGroups.Add(muscleGroup);
        _context.SaveChanges();

        _context.Equipment.Add(equipment);
        _context.SaveChanges();

        // Lưu thực thể được thêm vào danh sách
        _addedMuscleGroups.Add(muscleGroup);
        _addedEquipments.Add(equipment);

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
    public async Task Handle_ValidCommand_ReturnsSuccessResult()
    {
        // Arrange
        var muscleGroup = _addedMuscleGroups.First();
        var equipment = _addedEquipments.First();
        var name = "Exercise " + Guid.NewGuid().ToString();

        var command = new CreateExerciseCommand
        {
            CreatedBy = _user.Id,
            MuscleGroupIds = new List<int> { muscleGroup.MuscleGroupId },
            EquipmentId = equipment.EquipmentId,
            ExerciseName = name,
            DemoUrl = "http://example.com/demo",
            Type = "WeightResistance",
            Description = "Test Description",
            PublicVisibility = true
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();

        var exercise = await _context.Exercises
            .Include(e => e.ExerciseMuscleGroups)
            .FirstOrDefaultAsync(e => e.ExerciseName == command.ExerciseName);

        exercise.Should().NotBeNull();
        exercise!.CreatedBy.Should().Be(command.CreatedBy);
        exercise.EquipmentId.Should().Be(command.EquipmentId);
        exercise.ExerciseName.Should().Be(command.ExerciseName);
        exercise.DemoUrl.Should().Be(command.DemoUrl);
        exercise.Type.Should().Be(command.Type);
        exercise.Description.Should().Be(command.Description);
        exercise.PublicVisibility.Should().Be(command.PublicVisibility);
        exercise.ExerciseMuscleGroups.Select(emg => emg.MuscleGroupId).Should().BeEquivalentTo(command.MuscleGroupIds);

        // Lưu thực thể được thêm vào danh sách
        _addedExercises.Add(exercise);
    }



}


