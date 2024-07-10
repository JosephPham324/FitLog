//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using FitLog.Application.Common.Interfaces;
//using FitLog.Application.MuscleGroups.Commands.CreateMuscleGroup;
//using FitLog.Domain.Entities;
//using FitLog.Infrastructure.Data;
//using FluentAssertions;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.ChangeTracking;
//using Moq;
//using NUnit.Framework;

//namespace FitLog.Application.UnitTests.Use_Cases.MuscleGroups.Create;
//public class CreateMuscleGroupCommandHandlerTests
//{
//    private ApplicationDbContext _context;
//    private CreateMuscleGroupCommandHandler _handler;

//    [SetUp]
//    public void Setup()
//    {
//        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
//            .Options;

//        _context = new ApplicationDbContext(options);
//        _handler = new CreateMuscleGroupCommandHandler(_context);
//    }

//    [TearDown]
//    public void TearDown()
//    {
//        _context.Dispose();
//    }

//    [Test]
//    public async Task Handle_ValidCommand_ReturnsNewMuscleGroupId()
//    {
//        // Arrange
//        var command = new CreateMuscleGroupCommand
//        {
//            MuscleGroupName = "Legs",
//            ImageUrl = "http://example.com/image"
//        };

//        // Act
//        var result = await _handler.Handle(command, CancellationToken.None);

//        // Assert
//        result.Should().BeGreaterThan(0);

//        var muscleGroup = await _context.MuscleGroups.FindAsync(result);
//        muscleGroup.Should().NotBeNull();
//        muscleGroup!.MuscleGroupName.Should().Be(command.MuscleGroupName);
//        muscleGroup.ImageUrl.Should().Be(command.ImageUrl);
//    }
//}


