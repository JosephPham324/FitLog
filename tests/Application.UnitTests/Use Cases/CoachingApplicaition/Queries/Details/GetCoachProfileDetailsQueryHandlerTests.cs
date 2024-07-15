using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using AutoMapper;
using FitLog.Application.CoachProfiles.Queries.GetCoachProfileDetails;
using FitLog.Application.Use_Cases.CoachProfiles.Queries.GetCoachProfileDetails;
using FitLog.Domain.Entities;
using FitLog.Infrastructure.Data;
using FitLog.Infrastructure.Identity;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FitLog.Application.UnitTests.Use_Cases.CoachingApplicaition.Queries.Details;
public class GetCoachProfileDetailsQueryHandlerTests : IDisposable
{
    private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;
    private readonly IMapper _mapper;

    public GetCoachProfileDetailsQueryHandlerTests()
    {

        _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .Options;

        SeedSampleData();

        // AutoMapper configuration
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new CoachProfileDetailsDto.Mapping());
            cfg.AddProfile(new ProgramOverviewDto.Mapping());
        });
        _mapper = mapperConfig.CreateMapper();
    }

    private void SeedSampleData()
    {
        using (var context = new ApplicationDbContext(_dbContextOptions))
        {
            var user = context.AspNetUsers.FirstOrDefault(u=>u.Id == "user_id");


            var profile = new FitLog.Domain.Entities.Profile
            {
                UserId = user!.Id,
                Bio = "Test bio",
                ProfilePicture = "profile.jpg",
                MajorAchievements = new List<string> { "Achievement 1", "Achievement 2" },
                GalleryImageLinks = new List<string> { "image1.jpg", "image2.jpg" }
            };
            context.Profiles.Add(profile);

            var program1 = new Program
            {
                UserId = user.Id,
                ProgramName = "Program Test 1",
                NumberOfWeeks = 12,
                DaysPerWeek = 5,
                ExperienceLevel = "Intermediate",
                GymType = "Home Gym",
                MusclesPriority = "Upper Body"
            };
            context.Programs.Add(program1);

            var program2 = new Program
            {
                UserId = user.Id,
                ProgramName = "Program Test 2",
                NumberOfWeeks = 8,
                DaysPerWeek = 4,
                ExperienceLevel = "Beginner",
                GymType = "Commercial Gym",
                MusclesPriority= "Lower Body"
            };
            context.Programs.Add(program2);

            context.SaveChanges();
        }
    }

    public void Dispose()
    {
        using (var context = new ApplicationDbContext(_dbContextOptions))
        {
            var user = context.AspNetUsers.FirstOrDefault(u => u.Id == "user_id");

            if (user != null)
            {
                var programs = context.Programs.Where(p => p.UserId == user.Id).ToList();
                context.Programs.RemoveRange(programs);

                var profile = context.Profiles.FirstOrDefault(p => p.UserId == user.Id);
                if (profile != null)
                {
                    context.Profiles.Remove(profile);
                }

                context.SaveChanges();
            }
        }

    }



        [Fact]
    public async Task Handle_ReturnsCoachProfileDetails()
    {
        // Arrange
        var query = new GetCoachProfileDetailsQuery("user_id");

        using (var context = new ApplicationDbContext(_dbContextOptions))
        {
            var handler = new GetCoachProfileDetailsQueryHandler(context, _mapper);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            //result.ProfileId.Should().Be(1);
            result.UserId.Should().Be("user_id");
            result.Bio.Should().Be("Test bio");
            result.ProfilePicture.Should().Be("profile.jpg");
            result.MajorAchievements.Should().HaveCount(2);
            result.GalleryImageLinks.Should().HaveCount(2);

            result.ProgramsOverview.Should().NotBeNull();
            result.ProgramsOverview.Should().HaveCount(2);

            var programsOverview = result.ProgramsOverview;

            if (programsOverview != null)
            {
                var program1Id = await context.Programs
                .Where(p => p.UserId == "user_id" && p.ProgramName == "Program Test 1")
                .Select(p => p.ProgramId)
                .FirstOrDefaultAsync();

                var program2Id = await context.Programs
                    .Where(p => p.UserId == "user_id" && p.ProgramName == "Program Test 2")
                    .Select(p => p.ProgramId)
                    .FirstOrDefaultAsync();

                var program1 = programsOverview.FirstOrDefault(p => p.ProgramId == program1Id);
                program1.Should().NotBeNull();
                program1!.ProgramName.Should().Be("Program Test 1");
                program1.NumberOfWeeks.Should().Be(12);
                program1.DaysPerWeek.Should().Be(5);
                program1.ExperienceLevel.Should().Be("Intermediate");
                program1.GymType.Should().Be("Home Gym");
                //program1.MusclePriority.Should().Be("Upper Body");

                var program2 = programsOverview.FirstOrDefault(p => p.ProgramId == program2Id);
                program2.Should().NotBeNull();
                program2!.ProgramName.Should().Be("Program Test 2");
                program2.NumberOfWeeks.Should().Be(8);
                program2.DaysPerWeek.Should().Be(4);
                program2.ExperienceLevel.Should().Be("Beginner");
                program2.GymType.Should().Be("Commercial Gym");
                //program2.MusclePriority.Should().Be("Lower Body");
            }
            
        }
    }

    [Fact]
    public async Task Handle_ThrowsNotFoundException_WhenProfileNotFound()
    {
        // Arrange
        var query = new GetCoachProfileDetailsQuery("non_existing_user_id");

        using (var context = new ApplicationDbContext(_dbContextOptions))
        {
            var handler = new GetCoachProfileDetailsQueryHandler(context, _mapper);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(query, CancellationToken.None));
        }
    }
}

