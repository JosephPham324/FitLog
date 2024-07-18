using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitLog.Application.Equipments.Commands.Delete;
using FitLog.Domain.Entities;
using FitLog.Infrastructure.Data;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace FitLog.Application.UnitTests.Use_Cases.Equipments.Delete;
public class DeleteEquipmentCommandHandlerTests
{
    private ApplicationDbContext _context;
    private DeleteEquipmentCommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;

        _context = new ApplicationDbContext(options);
        _handler = new DeleteEquipmentCommandHandler(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }

    [Test]
    public async Task Handle_ValidCommand_DeletesEquipment()
    {
        // Arrange
        var equipment = new Equipment { EquipmentName = "Test Equipment", ImageUrl = "http://example.com/image.jpg" };
        _context.Equipment.Add(equipment);
        await _context.SaveChangesAsync();

        var command = new DeleteEquipmentCommand { EquipmentId = equipment.EquipmentId };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Success.Should().BeTrue();

        var deletedEquipment = await _context.Equipment.FindAsync(equipment.EquipmentId);
        deletedEquipment.Should().BeNull();
    }

    [Test]
    public async Task Handle_InvalidId_ReturnsFalse()
    {
        // Arrange
        var command = new DeleteEquipmentCommand { EquipmentId = 999 };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Success.Should().BeFalse();
    }
}

