using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using AutoMapper;
using FitLog.Application.Equipments.Queries.GetEquipmentDetails;
using FitLog.Application.Equipments.Queries.GetEquipmentsList;
using FitLog.Domain.Entities;
using FitLog.Infrastructure.Data;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace FitLog.Application.UnitTests.Use_Cases.Equipments.Queries.Details;
public class GetEquipmentDetailsQueryHandlerTests
{
    private ApplicationDbContext _context;
    private IMapper _mapper;
    private GetEquipmentDetailsQueryHandler _handler;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;

        _context = new ApplicationDbContext(options);

        var config = new MapperConfiguration(cfg => cfg.AddProfile(new EquipmentDetailsDTO.Mapping()));
        _mapper = config.CreateMapper();

        _handler = new GetEquipmentDetailsQueryHandler(_context, _mapper);
    }

    [TearDown]
    public void TearDown()
    {
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
    public async Task Handle_ValidQuery_ReturnsEquipmentDetails()
    {
        // Arrange
        var equipment = new Equipment { EquipmentName = "Test Equipment", ImageUrl = "http://example.com/image.jpg" };
        _context.Equipment.Add(equipment);
        await _context.SaveChangesAsync();

        var query = new GetEquipmentDetailsQuery { EquipmentId = equipment.EquipmentId };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.EquipmentId.Should().Be(equipment.EquipmentId);
        result.EquipmentName.Should().Be(equipment.EquipmentName);
        result.ImageUrl.Should().Be(equipment.ImageUrl);
    }

    [Test]
    public void Handle_InvalidId_ThrowsNotFoundException()
    {
        // Arrange
        var query = new GetEquipmentDetailsQuery { EquipmentId = 999 };

        // Act
        Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

        // Assert
        act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("Equipment (999) was not found.");
    }
}

