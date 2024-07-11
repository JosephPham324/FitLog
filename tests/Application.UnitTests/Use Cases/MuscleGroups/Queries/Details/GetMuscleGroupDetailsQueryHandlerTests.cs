//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Ardalis.GuardClauses;
//using FitLog.Application.Common.Interfaces;
//using FitLog.Application.MuscleGroups.Queries.GetMuscleGroupDetails;
//using FitLog.Domain.Entities;
//using FluentAssertions;
//using Moq;
//using NUnit.Framework;

//namespace FitLog.Application.UnitTests.Use_Cases.MuscleGroups.Queries.Details;
//public class GetMuscleGroupDetailsQueryHandlerTests
//{
//    private Mock<IApplicationDbContext> _contextMock;
//    private GetMuscleGroupDetailsQueryHandler _handler;

//    [SetUp]
//    public void Setup()
//    {
//        _contextMock = new Mock<IApplicationDbContext>();
//        _handler = new GetMuscleGroupDetailsQueryHandler(_contextMock.Object);
//    }

//    [Test]
//    public void Handle_ValidId_ShouldReturnCorrectMuscleGroupDTO()
//    {
//        // Arrange
//        var expectedMuscleGroup = new MuscleGroup
//        {
//            MuscleGroupId = 1,
//            MuscleGroupName = "Legs",
//            ImageUrl = "http://example.com/legs"
//        };

//        _contextMock.Setup(x => x.MuscleGroups.FindAsync(1)).ReturnsAsync(expectedMuscleGroup);

//        var query = new GetMuscleGroupDetailsQuery { MuscleGroupId = 1 };

//        // Act
//        var result = _handler.Handle(query, CancellationToken.None).Result;

//        // Assert
//        result.Should().NotBeNull();
//        result.MuscleGroupId.Should().Be(expectedMuscleGroup.MuscleGroupId);
//        result.MuscleGroupName.Should().Be(expectedMuscleGroup.MuscleGroupName);
//        result.ImageUrl.Should().Be(expectedMuscleGroup.ImageUrl);
//    }

//    [Test]
//    public void Handle_InvalidId_ShouldThrowNotFoundException()
//    {
//        // Arrange
//        _contextMock.Setup(x => x.MuscleGroups.FindAsync(999)).ReturnsAsync((MuscleGroup)null);

//        var query = new GetMuscleGroupDetailsQuery { Id = 999 };

//        // Act + Assert
//        Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(query, CancellationToken.None));
//    }
//}

