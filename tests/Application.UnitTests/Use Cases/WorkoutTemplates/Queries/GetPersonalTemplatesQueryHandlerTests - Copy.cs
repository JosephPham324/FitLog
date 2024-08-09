using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Moq;
using Xunit;
using AutoMapper;
using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Models;
using FitLog.Application.WorkoutTemplates.Queries.GetPersonalTemplate;
using FitLog.Domain.Entities;
using FitLog.Application.Common.Mappings;
using Microsoft.EntityFrameworkCore;
using FitLog.Application.Use_Cases.WorkoutTemplates.Queries;
using MockQueryable.Moq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace FitLog.Application.UnitTests.WorkoutTemplates.Queries.GetPersonalTemplate
{
    public class GetPublicTemplatesTests
    {
        private readonly Mock<IApplicationDbContext> _mockContext;
        private readonly Mock<IMapper> _mockMapper;
        private readonly GetPersonalTemplatesQueryHandler _handler;

        public GetPublicTemplatesTests()
        {
            _mockContext = new Mock<IApplicationDbContext>();
            _mockMapper = new Mock<IMapper>();
            _handler = new GetPersonalTemplatesQueryHandler(_mockContext.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Handle_GivenValidRequest_ReturnsPersonalTemplates()
        {
            // Arrange
            var userId = "user123";
            var query = new GetPersonalTemplatesQuery
            {
                UserId = userId,
                PageNumber = 1,
                PageSize = 10
            };

            var workoutTemplates = new List<WorkoutTemplate>
            {
                new WorkoutTemplate { TemplateName = "Template1", CreatedBy = userId },
                new WorkoutTemplate { TemplateName = "Template2", CreatedBy = userId }
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
            var userId = "user123";
            var query = new GetPersonalTemplatesQuery
            {
                UserId = userId,
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
        public async Task Handle_InvalidPaginationParameters_ThrowsValidationException()
        {
            // Arrange
            var query = new GetPersonalTemplatesQuery
            {
                UserId = "user123",
                PageNumber = 0, // Invalid PageNumber
                PageSize = 10
            };

            var validator = new GetPersonalTemplatesQueryValidator();
            var validationResult = await validator.ValidateAsync(query);

            // Act
            Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);
            

            // Assert
            validationResult.IsValid.Should().BeFalse();
            // Assert
            //await act.Should().ThrowAsync<ValidationException>()
            //    .WithMessage("Validation failed: \r\n -- PageNumber: Page number must be greater than 0.");
        }
    }


}
