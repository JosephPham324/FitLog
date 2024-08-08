//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using FitLog.Application.CoachProfiles.Queries.CreateCoachApplication;
//using FitLog.Application.Common.Interfaces;
//using FitLog.Application.Common.Models;
//using FitLog.Domain.Entities;
//using FluentAssertions;
//using Moq;
//using Xunit;

//namespace FitLog.Application.UnitTests.Use_Cases.CoachingApplicaition.Create;
//public class CreateCoachApplicationQueryHandlerTests
//{
//    private readonly Mock<IApplicationDbContext> _mockDbContext;
//    private readonly Mock<IUserTokenService> _mockUserTokenService;
//    private readonly CreateCoachApplicationQueryHandler _handler;

//    public CreateCoachApplicationQueryHandlerTests()
//    {
//        _mockDbContext = new Mock<IApplicationDbContext>();
//        _mockUserTokenService = new Mock<IUserTokenService>();
//        _handler = new CreateCoachApplicationQueryHandler(_mockDbContext.Object, _mockUserTokenService.Object);
//    }

//    [Fact]
//    public async Task Handle_GivenInvalidToken_ShouldReturnFailureResult()
//    {
//        // Arrange
//        var query = new CreateCoachApplicationQuery { Token = "invalid_token" };
//        _mockUserTokenService.Setup(x => x.GetUserIdFromGivenToken(It.IsAny<string>())).Returns((string?)null);

//        // Act
//        var result = await _handler.Handle(query, CancellationToken.None);

//        // Assert
//        result.Should().BeEquivalentTo(Result.Failure(new[] { "User is not authenticated" }));
//        _mockDbContext.Verify(x => x.CoachApplications.Add(It.IsAny<CoachApplication>()), Times.Never);
//        _mockDbContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
//    }

//    [Fact]
//    public async Task Handle_GivenValidToken_ShouldCreateCoachApplication()
//    {
//        // Arrange
//        var query = new CreateCoachApplicationQuery { Token = "valid_token" };
//        var userId = "user_id";
//        _mockUserTokenService.Setup(x => x.GetUserIdFromGivenToken(It.IsAny<string>())).Returns(userId);

//        _mockDbContext.Setup(x => x.CoachApplications.Add(It.IsAny<CoachApplication>())).Verifiable();
//        _mockDbContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1).Verifiable();

//        // Act
//        var result = await _handler.Handle(query, CancellationToken.None);

//        // Assert
//        result.Should().BeEquivalentTo(Result.Successful());
//        _mockDbContext.Verify(x => x.CoachApplications.Add(It.Is<CoachApplication>(ca => ca.ApplicantId == userId && ca.Status == "Pending")), Times.Once);
//        _mockDbContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
//    }

//}

