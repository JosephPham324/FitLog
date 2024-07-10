using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FitLog.Application.Common.Interfaces;
using FitLog.Application.MuscleGroups.Queries.GetMuscleGroupsListWithPagination;
using FitLog.Domain.Entities;
using FitLog.Infrastructure.Data;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace FitLog.Application.UnitTests.Use_Cases.MuscleGroups.Queries.GetAll;
public class GetMuscleGroupsListWithPaginationQueryHandlerTests
{
    private ApplicationDbContext _dbContext;
    private Mock<IMapper> _mapperMock;
    private GetMuscleGroupsListWithPaginationQueryHandler _handler;
    private List<MuscleGroup> _testMuscleGroups;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .Options;

        _dbContext = new ApplicationDbContext(options);

        _testMuscleGroups = new List<MuscleGroup>
        {
            new MuscleGroup { MuscleGroupName = "Group A", ImageUrl = "http://example.com/image1.jpg" },
            new MuscleGroup { MuscleGroupName = "Group B", ImageUrl = "http://example.com/image2.jpg" },
            new MuscleGroup { MuscleGroupName = "Group C", ImageUrl = "http://example.com/image3.jpg" }
        };
        _dbContext.MuscleGroups.AddRange(_testMuscleGroups);
        _dbContext.SaveChanges();

        var contextMock = new Mock<IApplicationDbContext>();
        contextMock.Setup(x => x.MuscleGroups).Returns(_dbContext.MuscleGroups);

        _mapperMock = new Mock<IMapper>();
        _handler = new GetMuscleGroupsListWithPaginationQueryHandler(contextMock.Object, _mapperMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        // Xóa những mục đã thêm trong quá trình kiểm tra
        _dbContext.MuscleGroups.RemoveRange(_testMuscleGroups);
        _dbContext.SaveChanges();
        _dbContext.Dispose();
    }

    [Test]
    public async Task Handle_ReturnsPaginatedList()
    {
        // Arrange
        var initialCount = _dbContext.MuscleGroups.Count() - _testMuscleGroups.Count; // Số lượng phần tử ban đầu
        var request = new GetMuscleGroupsListWithPaginationQuery
        {
            PageNumber = 1,
            PageSize = 10
        };

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(initialCount + _testMuscleGroups.Count); // Số lượng phần tử ban đầu cộng thêm 3

        var groupA = result.Items.FirstOrDefault(x => x.MuscleGroupName == "Group A");
        groupA.Should().NotBeNull();
        groupA!.MuscleGroupName.Should().Be("Group A");
        groupA.ImageUrl.Should().Be("http://example.com/image1.jpg");
    }
}






