using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitLog.Application.Equipments.Commands.UpdateEquipment;
using FitLog.Domain.Entities;
using FitLog.Infrastructure.Data;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace FitLog.Application.UnitTests.Use_Cases.Equipments.Update;
public class UpdateEquipmentCommandHandlerTests
{
    private ApplicationDbContext _context;
    private UpdateEquipmentCommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;

        _context = new ApplicationDbContext(options);
        _handler = new UpdateEquipmentCommandHandler(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }

    [Test]
    public async Task Handle_ValidCommand_UpdatesEquipment()
    {
        // Arrange
        var equipment = new Equipment { EquipmentName = "Old Equipment", ImageUrl = "http://example.com/old.jpg" };
        _context.Equipment.Add(equipment);
        await _context.SaveChangesAsync();

        var command = new UpdateEquipmentCommand { EquipmentId = equipment.EquipmentId, EquipmentName = "Updated Equipment", ImageUrl = "http://example.com/updated.jpg" };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();

        var updatedEquipment = await _context.Equipment.FindAsync(equipment.EquipmentId);
        updatedEquipment.Should().NotBeNull();
        updatedEquipment!.EquipmentName.Should().Be(command.EquipmentName);
        updatedEquipment!.ImageUrl.Should().Be(command.ImageUrl);
    }

    [Test]
    public async Task Handle_InvalidId_ReturnsFalse()
    {
        // Arrange
        var command = new UpdateEquipmentCommand { EquipmentId = -4, EquipmentName = "Updated Equipment", ImageUrl = "http://example.com/updated.jpg" };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Success.Should().BeFalse();
    }
}

