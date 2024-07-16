using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FitLog.Application.Common.Interfaces;
using FitLog.Application.Exercises.Queries.GetExercises;
using FitLog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace FitLog.Application.UnitTests.Use_Cases.Exercises.Queries.GetAll;
public class GetExercisesWithPaginationQueryHandlerTests
{
    private GetExercisesWithPaginationQueryHandler _handler;
    private Mock<IApplicationDbContext> _mockContext;
    private IMapper _mapper;

    [SetUp]
    public void Setup()
    {
        _mockContext = new Mock<IApplicationDbContext>();
        var mockMapper = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Exercise, ExerciseDTO>();
        });
        _mapper = mockMapper.CreateMapper();

        _handler = new GetExercisesWithPaginationQueryHandler(_mockContext.Object, _mapper);
    }

    [Test]
    public async Task Handle_ReturnsPaginatedList()
    {
        // Arrange
        var exercises = new List<Exercise>
            {
                new Exercise { ExerciseId = 1, ExerciseName = "Exercise 1", Type = "Weight Resistance" },
                new Exercise { ExerciseId = 2, ExerciseName = "Exercise 2", Type = "Calisthenics" },
                new Exercise { ExerciseId = 3, ExerciseName = "Exercise 3", Type = "Plyometrics" },
            }.AsQueryable();

        var pageNumber = 1;
        var pageSize = 1;

        _mockContext.Setup(c => c.Exercises).Returns(MockDbSet(exercises));

        var query = new GetExercisesWithPaginationQuery { PageNumber = pageNumber, PageSize = pageSize };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.That(result.Items.Count, Is.EqualTo(3)); // Assuming PageSize is large enough to cover all items in the mocked data
        Assert.That(result.PageNumber, Is.EqualTo(1));
        Assert.That(result.TotalCount, Is.EqualTo(3));
        Assert.That(result.TotalPages, Is.EqualTo(3));
    }

    [Test]
    public async Task Handle_ReturnsEmptyPaginatedList_WhenNoData()
    {
        // Arrange
        var exercises = new List<Exercise>().AsQueryable();

        _mockContext.Setup(c => c.Exercises).Returns(MockDbSet(exercises));

        var query = new GetExercisesWithPaginationQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.That(result.Items.Count, Is.EqualTo(0));
        Assert.That(result.PageNumber, Is.EqualTo(1));
        Assert.That(result.TotalCount, Is.EqualTo(0));
        Assert.That(result.TotalPages, Is.EqualTo(0));
    }

    private static DbSet<Exercise> MockDbSet(IQueryable<Exercise> exercises)
    {
        var mockDbSet = new Mock<DbSet<Exercise>>();
        mockDbSet.As<IQueryable<Exercise>>().Setup(m => m.Provider).Returns(exercises.Provider);
        mockDbSet.As<IQueryable<Exercise>>().Setup(m => m.Expression).Returns(exercises.Expression);
        mockDbSet.As<IQueryable<Exercise>>().Setup(m => m.ElementType).Returns(exercises.ElementType);
        mockDbSet.As<IQueryable<Exercise>>().Setup(m => m.GetEnumerator()).Returns(exercises.GetEnumerator());
        return mockDbSet.Object;
    }
}

