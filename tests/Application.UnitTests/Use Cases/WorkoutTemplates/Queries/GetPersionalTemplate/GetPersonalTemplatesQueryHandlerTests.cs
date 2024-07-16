using FitLog.Application.Common.Interfaces;
using FitLog.Application.WorkoutTemplates.Queries.GetPersonalTemplate;
using FitLog.Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

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
        var userToken = "valid_user_token";
        var userId = "user_id_from_token";

        _currentUserServiceMock.Setup(m => m.GetUserIdFromGivenToken(userToken))
                               .Returns(userId);

        var templates = new List<WorkoutTemplate>
        {
            new WorkoutTemplate { Id = 1, TemplateName = "Template 1", CreatedBy = userId, IsPublic = false },
            new WorkoutTemplate { Id = 2, TemplateName = "Template 2", CreatedBy = userId, IsPublic = false },
            new WorkoutTemplate { Id = 3, TemplateName = "Template 3", CreatedBy = userId, IsPublic = false }
        };

        var mockSet = new Mock<DbSet<WorkoutTemplate>>();
        mockSet.As<IQueryable<WorkoutTemplate>>().Setup(m => m.Provider).Returns(templates.AsQueryable().Provider);
        mockSet.As<IQueryable<WorkoutTemplate>>().Setup(m => m.Expression).Returns(templates.AsQueryable().Expression);
        mockSet.As<IQueryable<WorkoutTemplate>>().Setup(m => m.ElementType).Returns(templates.AsQueryable().ElementType);
        mockSet.As<IQueryable<WorkoutTemplate>>().Setup(m => m.GetEnumerator()).Returns(templates.GetEnumerator());

        _contextMock.Setup(m => m.WorkoutTemplates).Returns(mockSet.Object);

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
        var userToken = "valid_user_token";
        var userId = "user_id_from_token";

        _currentUserServiceMock.Setup(m => m.GetUserIdFromGivenToken(userToken))
                               .Returns(userId);

        var templates = new List<WorkoutTemplate>(); // Empty list

        var mockSet = new Mock<DbSet<WorkoutTemplate>>();
        mockSet.As<IQueryable<WorkoutTemplate>>().Setup(m => m.Provider).Returns(templates.AsQueryable().Provider);
        mockSet.As<IQueryable<WorkoutTemplate>>().Setup(m => m.Expression).Returns(templates.AsQueryable().Expression);
        mockSet.As<IQueryable<WorkoutTemplate>>().Setup(m => m.ElementType).Returns(templates.AsQueryable().ElementType);
        mockSet.As<IQueryable<WorkoutTemplate>>().Setup(m => m.GetEnumerator()).Returns(templates.GetEnumerator());

        _contextMock.Setup(m => m.WorkoutTemplates).Returns(mockSet.Object);

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
