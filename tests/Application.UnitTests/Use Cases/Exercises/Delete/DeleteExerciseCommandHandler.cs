using FitLog.Application.Exercises.Commands.CreateExercise;
using FitLog.Application.Exercises.Commands.DeleteExercise;
using FitLog.Application.Common.Models;
using FitLog.Domain.Entities;
using FitLog.Infrastructure.Data;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

public class DeleteExerciseCommandHandlerTests
{
    private ApplicationDbContext _context;
    private DeleteExerciseCommandHandler _handler;
    private CreateExerciseCommandHandler _handlerCreate;

    private List<MuscleGroup> _addedMuscleGroups;
    private List<Equipment> _addedEquipments;
    private List<Exercise> _addedExercises;

    [SetUp]
    public async Task Setup()
    {
        //var options = new DbContextOptionsBuilder<ApplicationDbContext>()
        //    .UseInMemoryDatabase(databaseName: "TestDatabase")
        //    .Options;

        _context = new ApplicationDbContext();

        _handler = new DeleteExerciseCommandHandler(_context);
        _handlerCreate = new CreateExerciseCommandHandler(_context);

        _addedMuscleGroups = new List<MuscleGroup>();
        _addedEquipments = new List<Equipment>();
        _addedExercises = new List<Exercise>();

        var muscleGroup = new MuscleGroup { MuscleGroupName = "Test Muscle Group", ImageUrl = "http://example.com/image.jpg" };
        var equipment = new Equipment { EquipmentName = "Test Equipment", ImageUrl = "http://example.com/image.jpg" };

        _context.MuscleGroups.Add(muscleGroup);
        await _context.SaveChangesAsync();

        _context.Equipment.Add(equipment);
        await _context.SaveChangesAsync();

        _addedMuscleGroups.Add(muscleGroup);
        _addedEquipments.Add(equipment);

        var user = _context.AspNetUsers.FirstOrDefault() ?? new AspNetUser
        {
            Id = "user_id",
            UserName = "TestUser",
            Email = "testuser@example.com"
        };

        if (_context.AspNetUsers.Find(user.Id) == null)
        {
            _context.AspNetUsers.Add(user);
            await _context.SaveChangesAsync();
        }

        var command = new CreateExerciseCommand
        {
            CreatedBy = user.Id,
            MuscleGroupIds = new List<int> { muscleGroup.MuscleGroupId },
            EquipmentId = equipment.EquipmentId,
            ExerciseName = "Test Exercise",
            DemoUrl = "http://example.com/demo",
            Type = "WeightResistance",
            Description = "Test Description",
            PublicVisibility = true
        };

        var result = await _handlerCreate.Handle(command!, CancellationToken.None);

        if (result.Success)
        {
            var exercise = await _context.Exercises
                .Include(e => e.ExerciseMuscleGroups)
                .FirstOrDefaultAsync(e => e.ExerciseName == command.ExerciseName);

            if (exercise != null)
            {
                _addedExercises.Add(exercise);
                Console.WriteLine("Setup is correct. MuscleGroupIds and EquipmentId are reflected correctly.");
            }
            else
            {
                Console.WriteLine("Failed to create exercise.");
            }
        }
        else
        {
            Console.WriteLine("CreateExerciseCommandHandler returned failure: " + string.Join(", ", result.Errors));
        }
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

        foreach (var exercise in _addedExercises)
        {
            var existingExercise = _context.Exercises.Find(exercise.ExerciseId);
            if (existingExercise != null)
            {
                _context.Exercises.Remove(existingExercise);
            }
        }

        _context.SaveChanges();
        _context.Dispose();
    }

    [Test]
    public async Task Handle_ValidCommand_ReturnsTrue()
    {
        var ex = _addedExercises[0];
        var command = new DeleteExerciseCommand
        {
            ExerciseId = ex.ExerciseId
        };

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Success.Should().BeTrue();

        var exercise = await _context.Exercises.FindAsync(ex.ExerciseId);
        exercise.Should().BeNull();
    }

    [Test]
    public async Task Handle_InvalidExerciseId_ReturnsFalse()
    {
        var command = new DeleteExerciseCommand
        {
            ExerciseId = 0
        };

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Success.Should().BeFalse();
    }
}
