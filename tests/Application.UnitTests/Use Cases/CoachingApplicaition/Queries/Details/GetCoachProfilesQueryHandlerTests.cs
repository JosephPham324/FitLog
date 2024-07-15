//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using AutoMapper;
//using FitLog.Application.CoachProfiles.Queries.GetCoachProfiles;
//using FitLog.Infrastructure.Data;
//using FluentAssertions;
//using Microsoft.EntityFrameworkCore;
//using Xunit;

//namespace FitLog.Application.UnitTests.Use_Cases.CoachingApplicaition.Queries.Details;
//public class GetCoachProfilesQueryHandlerIntegrationTests : IDisposable
//{
//    private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;
//    private readonly IMapper _mapper;

//    public GetCoachProfilesQueryHandlerIntegrationTests()
//    {


//        _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
//            .Options;

//        SeedSampleData();

//        var mapperConfig = new MapperConfiguration(cfg =>
//        {
//        });
//        _mapper = mapperConfig.CreateMapper();
//    }

//    private void SeedSampleData()
//    {
//        using (var context = new ApplicationDbContext(_dbContextOptions))
//        {
//            var coachProfiles = new List<CoachProfile>
//                {
//                    new CoachProfile { Id = 1, Name = "Coach 1" },
//                    new CoachProfile { Id = 2, Name = "Coach 2" }
//                };

//            context.CoachProfiles.AddRange(coachProfiles);
//            context.SaveChanges();
//        }
//    }

//    public void Dispose()
//    {
//        using (var context = new ApplicationDbContext(_dbContextOptions))
//        {
//            context.Database.EnsureDeleted();
//        }
//    }

//    [Fact]
//    public async Task Handle_ReturnsCoachProfiles()
//    {
//        // Arrange
//        var query = new GetCoachProfilesQuery();

//        using (var context = new ApplicationDbContext(_dbContextOptions))
//        {
//            var handler = new GetCoachProfilesQueryHandler(context, _mapper);

//            // Act
//            var result = await handler.Handle(query, CancellationToken.None);

//            // Assert
//            result.Should().NotBeNull();
//            result.Should().BeOfType<List<CoachProfile>>(); // Adjust type based on your implementation
//            var resultList = result as List<CoachProfile>;
//            resultList.Should().NotBeNull();
//            resultList.Should().HaveCount(2); // Assuming we have 2 coach profiles in sample data
//        }
//    }

//    [Fact]
//    public async Task Handle_ReturnsEmptyListIfNoProfiles()
//    {
//        // Arrange
//        var query = new GetCoachProfilesQuery();

//        // Clear existing data to simulate empty database
//        using (var context = new ApplicationDbContext(_dbContextOptions))
//        {
//            context.CoachProfiles.RemoveRange(context.CoachProfiles);
//            await context.SaveChangesAsync();
//        }

//        using (var context = new ApplicationDbContext(_dbContextOptions))
//        {
//            var handler = new GetCoachProfilesQueryHandler(context, _mapper);

//            // Act
//            var result = await handler.Handle(query, CancellationToken.None);

//            // Assert
//            result.Should().NotBeNull();
//            result.Should().BeOfType<List<CoachProfile>>(); // Adjust type based on your implementation
//            var resultList = result as List<CoachProfile>;
//            resultList.Should().NotBeNull();
//            resultList.Should().HaveCount(0); // Expecting an empty list
//        }
//    }
//}

