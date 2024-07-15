using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FitLog.Application.CoachProfiles.Queries.GetCoachApplicationsWithPagination;
using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Models;
using FitLog.Domain.Entities;
using FitLog.Infrastructure.Data;
using FitLog.Infrastructure.Identity;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace FitLog.Application.UnitTests.Use_Cases.CoachingApplicaition.Queries.GetAll;
public class GetCoachApplicationsWithPaginationQueryHandlerTests : IDisposable
{
    private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;
    private readonly IMapper _mapper;

    public GetCoachApplicationsWithPaginationQueryHandlerTests()
    {
        _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .Options;

        SeedSampleData();

        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new CoachApplicationDto.Mapping());
        });
        _mapper = mapperConfig.CreateMapper();
    }

    private void SeedSampleData()
    {
        using (var context = new ApplicationDbContext(_dbContextOptions))
        {
            var user = context.AspNetUsers.FirstOrDefault(u => u.Id == "user_id");
            var coachApplications = new List<CoachApplication>
                {
                    new CoachApplication
                    {
                        ApplicantId = "applicant1",
                        Status = "Pending",
                        StatusReason = "New application",
                        Created = DateTimeOffset.UtcNow,
                        LastModified = DateTimeOffset.UtcNow,
                        Applicant = user!
                    },
                    new CoachApplication
                    {
                        ApplicantId = "applicant2",
                        Status = "Approved",
                        StatusReason = "Experienced coach",
                        Created = DateTimeOffset.UtcNow,
                        LastModified = DateTimeOffset.UtcNow,
                        Applicant = user!
                    }
                };

            context.CoachApplications.AddRange(coachApplications);
            context.SaveChanges();
        }
    }

    public void Dispose()
    {
        using (var context = new ApplicationDbContext(_dbContextOptions))
        {
            var coachApplications = context.CoachApplications
                    .Where(ca => ca.ApplicantId == "applicant1" || ca.ApplicantId == "applicant2")
                    .ToList();

                context.CoachApplications.RemoveRange(coachApplications);
                context.SaveChanges();
        }
    }


    [Fact]
    public async Task Handle_ReturnsPaginatedList()
    {
        // Arrange
        var query = new GetCoachApplicationsWithPaginationQuery
        {
            PageNumber = 1,
            PageSize = 10
        };

        using (var context = new ApplicationDbContext(_dbContextOptions))
        {
            var handler = new GetCoachApplicationsWithPaginationQueryHandler(context, _mapper);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<PaginatedList<CoachApplicationDto>>();
            result.Items.Should().HaveCount(2); // Assuming we have 2 items in sample data
            result.PageNumber.Should().Be(query.PageNumber);
            //result.TotalPages.Should().Be(query.PageSize);
            result.TotalPages.Should().Be(1);
        }
    }

    [Fact]
    public async Task Handle_ReturnsEmptyPaginatedListIfNoData()
    {
        // Arrange
        var query = new GetCoachApplicationsWithPaginationQuery
        {
            PageNumber = 1,
            PageSize = 10
        };

        // Remove all data to simulate empty database
        using (var context = new ApplicationDbContext(_dbContextOptions))
        {
            context.CoachApplications.RemoveRange(context.CoachApplications);
            await context.SaveChangesAsync();
        }

        using (var context = new ApplicationDbContext(_dbContextOptions))
        {
            var handler = new GetCoachApplicationsWithPaginationQueryHandler(context, _mapper);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<PaginatedList<CoachApplicationDto>>();
            result.Items.Should().HaveCount(0); // Expecting no items
            result.PageNumber.Should().Be(query.PageNumber);
            //result.TotalPages.Should().Be(query.PageSize);
            result.TotalPages.Should().Be(0);
        }
    }
}
