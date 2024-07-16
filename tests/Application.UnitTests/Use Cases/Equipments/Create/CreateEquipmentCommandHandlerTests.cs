using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitLog.Application.Common.Models;
using FitLog.Application.Equipments.Commands.CreateEquipment;
using FitLog.Domain.Entities;
using FitLog.Infrastructure.Data;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace FitLog.Application.UnitTests.Use_Cases.Equipments.Create;
public class CreateEquipmentCommandHandlerTests
{
    private ApplicationDbContext _context;
    private CreateEquipmentCommandHandler _handler;
    private int _initialEquipmentCount;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .Options;

        _context = new ApplicationDbContext(options);
        _initialEquipmentCount = _context.Equipment.Count();
        _handler = new CreateEquipmentCommandHandler(_context);
    }

    [TearDown]
    public void TearDown()
    {
        // Xóa dữ liệu thử nghiệm được tạo trong quá trình test
        var testEquipments = _context.Equipment
            .Where(e => e.EquipmentName == "Test Equipment")
            .ToList();

        if (testEquipments.Any())
        {
            // Tìm và xóa tất cả các exercises liên quan đến thiết bị thử nghiệm
            var testExerciseIds = testEquipments.SelectMany(e => _context.Exercises.Where(ex => ex.EquipmentId == e.EquipmentId)).Select(ex => ex.ExerciseId).ToList();
            var testExercises = _context.Exercises.Where(ex => testExerciseIds.Contains(ex.ExerciseId)).ToList();

            if (testExercises.Any())
            {
                _context.Exercises.RemoveRange(testExercises);
            }

            _context.Equipment.RemoveRange(testEquipments);
            _context.SaveChanges();
        }

        _context.Dispose();
    }



    [Test]
    public async Task Handle_ValidCommand_ReturnsSuccessResult()
    {
        // Arrange
        var command = new CreateEquipmentCommand { EquipmentName = "Test Equipment", ImageUrl = "http://example.com/image.jpg" };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();

        var equipment = await _context.Equipment.FirstOrDefaultAsync(e => e.EquipmentName == command.EquipmentName && e.ImageUrl == command.ImageUrl);
        equipment.Should().NotBeNull();

        // Xóa thiết bị thử nghiệm đã tạo
        _context.Equipment.Remove(equipment!);
        await _context.SaveChangesAsync();
    }


}




