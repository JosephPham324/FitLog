using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitLog.Application.MuscleGroups.Commands.UpdateMuscleGroup;
using FitLog.Domain.Entities;
using FitLog.Infrastructure.Data;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace FitLog.Application.UnitTests.Use_Cases.MuscleGroups.Update;
public class UpdateMuscleGroupCommandHandlerTests
{
    private ApplicationDbContext _context;
    private UpdateMuscleGroupCommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .Options;

        _context = new ApplicationDbContext(options);
        _handler = new UpdateMuscleGroupCommandHandler(_context);
    }

    [TearDown]
    public async Task TearDown()
    {
        await CleanUpTestDataAsync();
        _context.Dispose();
    }

    private async Task CleanUpTestDataAsync()
    {
        var testEntities = await _context.MuscleGroups
                                        .Where(m => m.MuscleGroupName!.StartsWith("Test ") ||
                                        m.MuscleGroupName!.StartsWith("Update "))
                                        .ToListAsync();

        _context.MuscleGroups.RemoveRange(testEntities);
        await _context.SaveChangesAsync();
    }

    [Test]
    public async Task Handle_ExistingId_ShouldReturnSuccess()
    {
        // Arrange
        var entity = new MuscleGroup { MuscleGroupName = "Test Handle" };
        await _context.MuscleGroups.AddAsync(entity);
        await _context.SaveChangesAsync();

        var command = new UpdateMuscleGroupCommand
        {
            Id = entity.MuscleGroupId,
            MuscleGroupName = "Updated Handle",
            ImageUrl = "http://example.com/updated"
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();

        var updatedEntity = await _context.MuscleGroups.FindAsync(entity.MuscleGroupId);
        updatedEntity.Should().NotBeNull();
        updatedEntity!.MuscleGroupName.Should().Be(command.MuscleGroupName);
        updatedEntity!.ImageUrl.Should().Be(command.ImageUrl);
    }

    [Test]
    public async Task Handle_NonExistingId_ShouldReturnFailure()
    {
        // Arrange
        var command = new UpdateMuscleGroupCommand
        {
            Id = 999,
            MuscleGroupName = "Updated Legs",
            ImageUrl = "http://example.com/updated"
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeFalse();
        result.Errors.Should().Contain("Muscle group not found.");

        var updatedEntity = await _context.MuscleGroups.FindAsync(999);
        updatedEntity.Should().BeNull();
    }

    [Test]
    public async Task Handle_ExceptionThrown_ShouldReturnFailure()
    {
        // Arrange
        var entity = new MuscleGroup { MuscleGroupName = "Test Exeption" };
        await _context.MuscleGroups.AddAsync(entity);
        await _context.SaveChangesAsync();

        var command = new UpdateMuscleGroupCommand
        {
            Id = entity.MuscleGroupId,
            MuscleGroupName = "Updated Exeption",
            ImageUrl = "http://example.com/updated"
        };

        // Simulate an exception
        _context.Entry(entity).State = EntityState.Detached; // Detach to force exception

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeFalse();
        result.Errors.Should().Contain("An error occurred while saving the muscle group.");

        var updatedEntity = await _context.MuscleGroups.FindAsync(entity.MuscleGroupId);
        updatedEntity.Should().NotBeNull(); // Entity should not be updated due to exception
        updatedEntity!.MuscleGroupName.Should().Be("Test Exeption"); // Should remain unchanged
    }
}

