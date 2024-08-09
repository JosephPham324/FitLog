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
using FitLog.Application.WorkoutTemplates.Queries.GetPublicTemplates;
using FitLog.Application.WorkoutTemplates.Queries.GetPersonalTemplate;
using FitLog.Domain.Entities;
using MockQueryable.Moq;
using FitLog.Application.Use_Cases.WorkoutTemplates.Queries;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace FitLog.Application.UnitTests.WorkoutTemplates.Queries.GetPublicTemplates
{
    public class GetPublicTemplatesQueryHandlerTests
    {
        private readonly Mock<IApplicationDbContext> _mockContext;
        private readonly Mock<IMapper> _mockMapper;
        private readonly GetPublicTemplatesQueryHandler _handler;

        public GetPublicTemplatesQueryHandlerTests()
        {
            _mockContext = new Mock<IApplicationDbContext>();
            _mockMapper = new Mock<IMapper>();
            _handler = new GetPublicTemplatesQueryHandler(_mockContext.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Handle_GivenValidRequest_ReturnsPublicTemplates()
        {
            // Arrange
            var query = new GetPublicTemplatesQuery
            {
                PageNumber = 1,
                PageSize = 10
            };

            var workoutTemplates = new List<WorkoutTemplate>
            {
                new WorkoutTemplate { TemplateName = "Template1", IsPublic = true },
                new WorkoutTemplate { TemplateName = "Template2", IsPublic = true }
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
            var query = new GetPublicTemplatesQuery
            {
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
            var query = new GetPublicTemplatesQuery
            {
                PageNumber = 0, // Invalid PageNumber
                PageSize = 10
            };

            var validator = new GetPublicTemplatesQueryValidator();
            var validationResult = await validator.ValidateAsync(query);

            // Assert
            validationResult.IsValid.Should().BeFalse();
        }
    }
}
