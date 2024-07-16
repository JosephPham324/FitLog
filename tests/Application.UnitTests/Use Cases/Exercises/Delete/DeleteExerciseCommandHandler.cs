using FitLog.Application.Exercises.Commands.CreateExercise;
using FitLog.Application.Exercises.Commands.DeleteExercise;
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
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
         //.UseInMemoryDatabase(databaseName: "TestDatabase")
         .Options;

        _context = new ApplicationDbContext(options);

        _handler = new DeleteExerciseCommandHandler(_context);
        _handlerCreate = new CreateExerciseCommandHandler(_context);

        // Thiết lập danh sách để lưu thực thể được thêm vào
        _addedMuscleGroups = new List<MuscleGroup>();
        _addedEquipments = new List<Equipment>();
        _addedExercises = new List<Exercise>();


        // Thiết lập dữ liệu test cho MuscleGroups và Equipment
        var muscleGroup = new MuscleGroup { MuscleGroupName = "Test Muscle Group", ImageUrl = "http://example.com/image.jpg" };
        var equipment = new Equipment { EquipmentName = "Test Equipment", ImageUrl = "http://example.com/image.jpg" };

        _context.MuscleGroups.Add(muscleGroup);
        await _context.SaveChangesAsync();

        _context.Equipment.Add(equipment);
        await _context.SaveChangesAsync();

        // Lưu thực thể được thêm vào danh sách
        _addedMuscleGroups.Add(muscleGroup);
        _addedEquipments.Add(equipment);
        var user = _context.AspNetUsers.FirstOrDefault();

        // Thêm một Exercise vào cơ sở dữ liệu và lấy ExerciseId
        var command = new CreateExerciseCommand
        {
            CreatedBy = user?.Id,
            MuscleGroupIds = new List<int> { muscleGroup.MuscleGroupId },
            EquipmentId = equipment.EquipmentId,
            ExerciseName = "Test Exercise",
            DemoUrl = "http://example.com/demo",
            Type = "WeightResistance",
            Description = "Test Description",
            PublicVisibility = true
        };

        var result = await _handlerCreate.Handle(command!, CancellationToken.None);
        var exercise = await _context.Exercises
            .Include(e => e.ExerciseMuscleGroups)
            .FirstOrDefaultAsync(e => e.ExerciseName == command.ExerciseName);

        if (exercise != null)
        {
            _addedExercises.Add(exercise);
            // Validate if the MuscleGroupIds and EquipmentId are reflected correctly
            if (exercise.ExerciseMuscleGroups.Any(mg => mg.MuscleGroupId == muscleGroup.MuscleGroupId) &&
                exercise.EquipmentId == equipment.EquipmentId)
            {
                Console.WriteLine("Setup is correct. MuscleGroupIds and EquipmentId are reflected correctly.");
            }
            else
            {
                Console.WriteLine("Setup is incorrect. There is a mismatch in MuscleGroupIds or EquipmentId.");
            }
        }
        else
        {
            Console.WriteLine("Failed to create exercise.");
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

        //if (_addedExercises.Any())
        //{
        //    _context.Exercises.RemoveRange(_addedExercises);
        //}

        _context.SaveChanges();
        _context.Dispose();
    }

    [Test]
    public async Task Handle_ValidCommand_ReturnsTrue()
    {
        var ex = _addedExercises[0];
        // Arrange
        var command = new DeleteExerciseCommand
        {
            ExerciseId = ex.ExerciseId // Sử dụng ExerciseId đã lấy được
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Success.Should().BeTrue();

        var exercise = await _context.Exercises.FindAsync(ex.ExerciseId);
        exercise.Should().BeNull();
    }


    [Test]
    public async Task Handle_InvalidExerciseId_ReturnsFalse()
    {
        // Arrange
        var command = new DeleteExerciseCommand
        {
            ExerciseId = 0 // Non-existent ExerciseId
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Success.Should().BeFalse();
    }
}

