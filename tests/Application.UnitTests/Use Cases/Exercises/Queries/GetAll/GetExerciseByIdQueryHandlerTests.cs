//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Ardalis.GuardClauses;
//using AutoMapper;
//using FitLog.Application.Exercises.Queries.GetExerciseDetails;
//using FitLog.Domain.Entities;
//using FitLog.Infrastructure.Data;
//using FitLog.Infrastructure.Identity;
//using FluentAssertions;
//using Microsoft.EntityFrameworkCore;
//using NUnit.Framework;

//namespace FitLog.Application.UnitTests.Use_Cases.Exercises.Queries.GetAll;
//public class GetExerciseByIdQueryHandlerTests
//{
//    private ApplicationDbContext _context;
//    private IMapper _mapper;
//    private GetExerciseByIdQueryHandler _handler;

//    [SetUp]
//    public void Setup()
//    {
//        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
//            .Options;

//        _context = new ApplicationDbContext(options);

//        var config = new MapperConfiguration(cfg =>
//        {
//            cfg.AddProfile(new ExerciseDetailsDTO.Mapping());
//        });
//        _mapper = config.CreateMapper();

//        var exercise = new Exercise
//        {
//            ExerciseId = 1,
//            CreatedBy = "user1",
//            MuscleGroupId = 1,
//            EquipmentId = 1,
//            ExerciseName = "Test Exercise",
//            DemoUrl = "http://example.com/demo",
//            Type = "WeightResistance"
//        };

//        var user = new ApplicationUser
//        {
//            Id = "user1",
//            UserName = "TestUser"
//        };

//        var muscleGroup = new MuscleGroup
//        {
//            MuscleGroupId = 1,
//            MuscleGroupName = "Test Muscle Group"
//        };

//        var equipment = new Equipment
//        {
//            EquipmentId = 1,
//            EquipmentName = "Test Equipment"
//        };

//        _context.Exercises.Add(exercise);
//        _context.MuscleGroups.Add(muscleGroup);
//        _context.Equipment.Add(equipment);
//        _context.SaveChanges();

//        _handler = new GetExerciseByIdQueryHandler(_context, _mapper);
//    }

//    [Test]
//    public async Task Handle_ValidId_ReturnsExerciseDetailsDTO()
//    {
//        // Arrange
//        var query = new GetExerciseByIdQuery { Id = 1 };

//        // Act
//        var result = await _handler.Handle(query, CancellationToken.None);

//        // Assert
//        result.Should().NotBeNull();
//        result.ExerciseId.Should().Be(1);
//        result.CreatedByName.Should().Be("TestUser");
//        result.MuscleGroupName.Should().Be("Test Muscle Group");
//        result.EquipmentName.Should().Be("Test Equipment");
//    }

//    [Test]
//    public void Handle_InvalidId_ThrowsNotFoundException()
//    {
//        // Arrange
//        var query = new GetExerciseByIdQuery { Id = 99 };

//        // Act
//        Func<Task> act = async () => { await _handler.Handle(query, CancellationToken.None); };

//        // Assert
//        act.Should().ThrowAsync<NotFoundException>().WithMessage("Exercise (99) was not found.");
//    }

//    [TearDown]
//    public void TearDown()
//    {
//        _context.Dispose();
//    }
//}

