//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using FitLog.Application.MuscleGroups.Commands.DeleteMuscleGroup;
//using FitLog.Domain.Entities;
//using FitLog.Infrastructure.Data;
//using FluentAssertions;
//using Microsoft.EntityFrameworkCore;
//using NUnit.Framework;

//namespace FitLog.Application.UnitTests.Use_Cases.MuscleGroups.Delete;
//public class DeleteMuscleGroupCommandHandlerTests
//{
//    private ApplicationDbContext _context;
//    private DeleteMuscleGroupCommandHandler _handler;

//    [SetUp]
//    public void Setup()
//    {
//        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
//            .Options;

//        _context = new ApplicationDbContext(options);
//        _handler = new DeleteMuscleGroupCommandHandler(_context);
//    }

//    [TearDown]
//    public void TearDown()
//    {
//        _context.Dispose();
//    }

//    [Test]
//    public async Task Handle_ExistingId_ShouldReturnTrueAndDeleteEntity()
//    {
//        // Arrange
//        var entity = new MuscleGroup { MuscleGroupName = "Legs" };
//        await _context.MuscleGroups.AddAsync(entity);
//        await _context.SaveChangesAsync();

//        var command = new DeleteMuscleGroupCommand { Id = entity.MuscleGroupId };

//        // Act
//        var result = await _handler.Handle(command, CancellationToken.None);

//        // Assert
//        result.Should().BeTrue();

//        var deletedEntity = await _context.MuscleGroups.FindAsync(entity.MuscleGroupId);
//        deletedEntity.Should().BeNull();
//    }

//    [Test]
//    public async Task Handle_NonExistingId_ShouldReturnFalse()
//    {
//        // Arrange
//        var command = new DeleteMuscleGroupCommand { Id = 999 };

//        // Act
//        var result = await _handler.Handle(command, CancellationToken.None);

//        // Assert
//        result.Should().BeFalse();
//    }

//    [Test]
//    public async Task Handle_ExceptionThrown_ShouldReturnFalse()
//    {
//        // Arrange
//        var command = new DeleteMuscleGroupCommand { Id = 0 };

//        // Act
//        var result = await _handler.Handle(command, CancellationToken.None);

//        // Assert
//        result.Should().BeFalse();
//    }
//}

