using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FitLog.Application.Equipments.Queries.GetEquipmentsList;
using FitLog.Domain.Entities;
using FitLog.Infrastructure.Data;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace FitLog.Application.UnitTests.Use_Cases.Equipments.Queries.GetList;
public class GetEquipmentsListWithPaginationQueryHandlerTests
{
    private ApplicationDbContext _context;
    private IMapper _mapper;
    private GetEquipmentsListWithPaginationQueryHandler _handler;
    private int _initialEquipmentCount;
    private int pageNumber;
    private int pageSize;
    private int totalTest;

    [SetUp]
    public async Task Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .Options;

        _context = new ApplicationDbContext(options);

        _initialEquipmentCount = await _context.Equipment.CountAsync();

        var config = new MapperConfiguration(cfg => cfg.AddProfile(new EquipmentDTO.Mapping()));
        _mapper = config.CreateMapper();

        _handler = new GetEquipmentsListWithPaginationQueryHandler(_context, _mapper);

        pageNumber = 1;
        pageSize = 10;
        totalTest = 20;

        // Seed test data
        for (int i = 1; i <= totalTest; i++)
        {
            _context.Equipment.Add(new Equipment { EquipmentName = $"Test Equipment {i}" });
        }
        await _context.SaveChangesAsync();
    }

    [TearDown]
    public async Task TearDown()
    {
        // Remove test data
        var testEquipments = _context.Equipment.Where(e => e.EquipmentName!.StartsWith("Test"));
        _context.Equipment.RemoveRange(testEquipments);
        await _context.SaveChangesAsync();

        _context.Dispose();
    }

    [Test]
    public async Task Handle_ValidQuery_ReturnsPaginatedList()
    {

        // Arrange
        var query = new GetEquipmentsWithPaginationQuery { PageNumber = pageNumber, PageSize = pageSize };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(pageSize);
        result.TotalCount.Should().Be(_initialEquipmentCount + totalTest);
        result.PageNumber.Should().Be(pageNumber);
        result.TotalPages.Should().Be((_initialEquipmentCount + totalTest + 9) / pageSize); // Round up to the next integer
    }

    [Test]
    public async Task Handle_InvalidPageNumber_ReturnsEmptyList()
    {
        // Arrange
        var query = new GetEquipmentsWithPaginationQuery { PageNumber = 99, PageSize = pageSize };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().BeEmpty();
        result.TotalCount.Should().Be(_initialEquipmentCount + totalTest);
        result.PageNumber.Should().Be(99);
        result.TotalPages.Should().Be((_initialEquipmentCount + totalTest + 9) / pageSize); // Round up to the next integer
    }
}


