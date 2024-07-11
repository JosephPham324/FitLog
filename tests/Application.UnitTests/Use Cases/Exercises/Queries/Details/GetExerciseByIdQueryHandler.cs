using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using AutoMapper;
using FitLog.Application.Exercises.Queries.GetExerciseDetails;
using FitLog.Domain.Entities;
using FitLog.Infrastructure.Data;
using FitLog.Infrastructure.Identity;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace FitLog.Application.UnitTests.Use_Cases.Exercises.Queries.Details
{
    public class GetExerciseByIdQueryHandlerTests
    {
        private ApplicationDbContext _context;
        private IMapper _mapper;
        private GetExerciseByIdQueryHandler _handler;
        private List<MuscleGroup> _addedMuscleGroups;
        private List<Equipment> _addedEquipments;
        private List<Exercise> _addedExercises;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .Options;

            _context = new ApplicationDbContext(options);
            // Thiết lập danh sách để lưu thực thể được thêm vào
            _addedMuscleGroups = new List<MuscleGroup>();
            _addedEquipments = new List<Equipment>();
            _addedExercises = new List<Exercise>();

            var muscleGroup = new MuscleGroup { MuscleGroupName = "Test Muscle Group" };
            var equipment = new Equipment { EquipmentName = "Test Equipment", ImageUrl = "http://example.com/image.jpg" };

            _context.MuscleGroups.Add(muscleGroup);
            _context.Equipment.Add(equipment);

            _addedEquipments.Add(equipment);
            _addedMuscleGroups.Add(muscleGroup);
            _context.SaveChanges();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ExerciseDetailsDTO.Mapping());
            });
            _mapper = config.CreateMapper();

            _handler = new GetExerciseByIdQueryHandler(_context, _mapper);

            var exercise = new Exercise
            {
                CreatedBy = "48fd07f4-2a6a-46ec-a577-db456fac44ce",
                EquipmentId = equipment.EquipmentId,
                ExerciseName = "Test Exercise",
                DemoUrl = "http://example.com/demo",
                Type = "WeightResistance",
                Description = "Test Description",
                PublicVisibility = true
            };

            _context.Exercises.Add(exercise);
            _context.SaveChanges();

            _addedExercises.Add(exercise);
        }

        [TearDown]
        public async Task TearDown()
        {
            
            var equipments = _context.Equipment.ToList();
            if (equipments.Any())
            {
                _context.Equipment.RemoveRange(equipments);
            }

            var exercises = _context.Exercises.ToList();
            if (exercises.Any())
            {
                _context.Exercises.RemoveRange(exercises);
            }

            //var muscleGroups = _context.MuscleGroups.ToList();
            //if (muscleGroups.Any())
            //{
            //    _context.MuscleGroups.RemoveRange(muscleGroups);
            //}

            await _context.SaveChangesAsync();

            await _context.DisposeAsync();
        }



        [Test]
        public async Task Handle_ValidId_ReturnsExerciseDetailsDTO()
        {
            // Arrange
            var query = new GetExerciseByIdQuery { Id = _addedExercises[0].ExerciseId };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.ExerciseId.Should().Be(_addedExercises[0].ExerciseId);
            result.CreatedByName.Should().Be("administrator@localhost");
            result.EquipmentName.Should().Be("Test Equipment");
        }

        [Test]
        public void Handle_InvalidId_ThrowsNotFoundException()
        {
            // Arrange
            var query = new GetExerciseByIdQuery { Id = 99 };

            // Act
            Func<Task> act = async () => { await _handler.Handle(query, CancellationToken.None); };

            // Assert
            act.Should().ThrowAsync<NotFoundException>().WithMessage("Exercise (99) was not found.");
        }
    }
}
