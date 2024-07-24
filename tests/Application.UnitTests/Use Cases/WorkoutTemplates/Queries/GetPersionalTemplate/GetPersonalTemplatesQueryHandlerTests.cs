using System.Linq.Expressions;
using FitLog.Application.Common.Interfaces;
using FitLog.Application.WorkoutTemplates.Queries.GetPersonalTemplate;
using FitLog.Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using MockQueryable.Moq;

public class GetPersonalTemplatesQueryHandlerTests
{
    private readonly Mock<IApplicationDbContext> _contextMock;
    private readonly Mock<IUserTokenService> _currentUserServiceMock;
    private readonly GetPersonalTemplatesQueryHandler _handler;

    public GetPersonalTemplatesQueryHandlerTests()
    {
        _contextMock = new Mock<IApplicationDbContext>();
        _currentUserServiceMock = new Mock<IUserTokenService>();
        _handler = new GetPersonalTemplatesQueryHandler(_contextMock.Object, _currentUserServiceMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidQuery_ShouldReturnPersonalTemplates()
    {
        // Arrange
        var userToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMDUxNTY0MjAwODQwMTM2ODkyNzQiLCJlbWFpbCI6InF1YW5ncG5jZTE3MDAzNkBmcHQuZWR1LnZuIiwiSWQiOiIzOTU0MDg4NC01NzRhLTQwMmYtYjMyYy1mNWRlODMzYmI2N2EiLCJleHAiOjE3MjE2Mjg2ODYsImlzcyI6Imh0dHBzOi8vbG9jYWxob3N0OjQ0NDQ3L2FwaSIsImF1ZCI6Imh0dHBzOi8vbG9jYWxob3N0OjQ0NDQ3LyJ9.-vaVzF57j5M9n-GPStxKtW41_WPcSAR-Me4eHN6MNjM";
        var userId = "39540884-574a-402f-b32c-f5de833bb67a";

        _currentUserServiceMock.Setup(m => m.GetUserIdFromGivenToken(userToken))
                               .Returns(userId);

        var templates = new List<WorkoutTemplate>
        {
            new WorkoutTemplate { Id = 1, TemplateName = "Template 1", CreatedBy = userId, IsPublic = false },
            new WorkoutTemplate { Id = 2, TemplateName = "Template 2", CreatedBy = userId, IsPublic = false },
            new WorkoutTemplate { Id = 3, TemplateName = "Template 3", CreatedBy = userId, IsPublic = false }
        }.AsQueryable().BuildMockDbSet();

        _contextMock.Setup(m => m.WorkoutTemplates).Returns(templates.Object);

        var query = new GetPersonalTemplatesQuery
        {
            UserToken = userToken,
            PageNumber = 1,
            PageSize = 10
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(3); // Ensure all templates for the user are returned
        result.Items.Select(wt => wt.CreatedBy).Should().OnlyContain(id => id == userId); // Ensure all templates belong to the correct user
    }

    [Fact]
    public async Task Handle_WithNoTemplates_ShouldReturnEmptyList()
    {
        // Arrange
        var userToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMDUxNTY0MjAwODQwMTM2ODkyNzQiLCJlbWFpbCI6InF1YW5ncG5jZTE3MDAzNkBmcHQuZWR1LnZuIiwiSWQiOiIzOTU0MDg4NC01NzRhLTQwMmYtYjMyYy1mNWRlODMzYmI2N2EiLCJleHAiOjE3MjE2Mjg2ODYsImlzcyI6Imh0dHBzOi8vbG9jYWxob3N0OjQ0NDQ3L2FwaSIsImF1ZCI6Imh0dHBzOi8vbG9jYWxob3N0OjQ0NDQ3LyJ9.-vaVzF57j5M9n-GPStxKtW41_WPcSAR-Me4eHN6MNjM";
        var userId = "39540884-574a-402f-b32c-f5de833bb67a";

        _currentUserServiceMock.Setup(m => m.GetUserIdFromGivenToken(userToken))
                               .Returns(userId);

        var templates = new List<WorkoutTemplate>().AsQueryable().BuildMockDbSet(); // Empty list

        _contextMock.Setup(m => m.WorkoutTemplates).Returns(templates.Object);

        var query = new GetPersonalTemplatesQuery
        {
            UserToken = userToken,
            PageNumber = 1,
            PageSize = 10
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().BeEmpty(); // Ensure no templates are returned
    }
}
