using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Moq;
using Xunit;
using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Models;
using FitLog.Application.WorkoutTemplates.Queries.FilterWorkoutTemplates;
using FitLog.Application.WorkoutTemplates.Queries.GetPersonalTemplate;
using FitLog.Domain.Entities;
using MockQueryable.Moq;
using FitLog.Application.Use_Cases.WorkoutTemplates.Queries;


namespace FitLog.Application.UnitTests.Use_Cases.WorkoutTemplates.Queries;
public class FilterWorkoutTemplatesQueryHandlerTests
{
    private readonly Mock<IApplicationDbContext> _mockContext;
    private readonly Mock<IMapper> _mockMapper;
    private readonly FilterWorkoutTemplatesQueryHandler _handler;

    public FilterWorkoutTemplatesQueryHandlerTests()
    {
        _mockContext = new Mock<IApplicationDbContext>();
        _mockMapper = new Mock<IMapper>();
        _handler = new FilterWorkoutTemplatesQueryHandler(_mockContext.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task Handle_GivenValidRequest_ReturnsFilteredTemplates()
    {
        // Arrange
        var query = new FilterWorkoutTemplatesQuery
        {
            TemplateName = "Template",
            CreatorName = "Creator",
            MinDuration = "00:30",
            MaxDuration = "01:30",
            PageNumber = 1,
            PageSize = 10
        };

        var workoutTemplates = new List<WorkoutTemplate>
            {
                new WorkoutTemplate { TemplateName = "Template1", CreatedByNavigation = new AspNetUser { UserName = "Creator1" }, Duration = "01:00" },
                new WorkoutTemplate { TemplateName = "Template2", CreatedByNavigation = new AspNetUser { UserName = "Creator2" }, Duration = "01:10" }
            }.AsQueryable().BuildMockDbSet();

        _mockContext.Setup(x => x.WorkoutTemplates).Returns(workoutTemplates.Object);

        var workoutTemplateDtos = new List<WorkoutTemplateListDto>
            {
                new WorkoutTemplateListDto { TemplateName = "Template1" },
                new WorkoutTemplateListDto { TemplateName = "Template2" }
            };

        _mockMapper.Setup(m => m.ConfigurationProvider)
            .Returns(new MapperConfiguration(cfg => cfg.CreateMap<WorkoutTemplate, WorkoutTemplateListDto>()));

        _mockMapper.Setup(m => m.ProjectTo<WorkoutTemplateListDto>(It.IsAny<IQueryable<WorkoutTemplate>>(), null))
            .Returns(workoutTemplateDtos.AsQueryable());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(2);
        result.Items.First().TemplateName.Should().Be("Template1");
        result.Items.Last().TemplateName.Should().Be("Template2");
    }

    [Fact]
    public async Task Handle_GivenNoTemplatesFound_ReturnsEmptyPaginatedList()
    {
        // Arrange
        var query = new FilterWorkoutTemplatesQuery
        {
            TemplateName = "Template",
            CreatorName = "Creator",
            MinDuration = "00:30",
            MaxDuration = "01:30",
            PageNumber = 1,
            PageSize = 10
        };

        var workoutTemplates = new List<WorkoutTemplate>().AsQueryable().BuildMockDbSet();

        _mockContext.Setup(x => x.WorkoutTemplates).Returns(workoutTemplates.Object);

        var workoutTemplateDtos = new List<WorkoutTemplateListDto>();

        _mockMapper.Setup(m => m.ConfigurationProvider)
            .Returns(new MapperConfiguration(cfg => cfg.CreateMap<WorkoutTemplate, WorkoutTemplateListDto>()));

        _mockMapper.Setup(m => m.ProjectTo<WorkoutTemplateListDto>(It.IsAny<IQueryable<WorkoutTemplate>>(), null))
            .Returns(workoutTemplateDtos.AsQueryable());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_InvalidPaginationParameters_ValidationReturnsFalse()
    {
        // Arrange
        var query = new FilterWorkoutTemplatesQuery
        {
            PageNumber = 0, // Invalid PageNumber
            PageSize = 10
        };

        var validator = new FilterWorkoutTemplatesQueryValidator();
        var validationResult = await validator.ValidateAsync(query);

        // Assert
        validationResult.IsValid.Should().BeFalse();
    }
}
