using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FitLog.Application.Common.Interfaces;
using FitLog.Application.CoachProfiles.Queries.GetCoachProfileDetails;
using FitLog.Application.Use_Cases.CoachProfiles.Queries.GetCoachProfileDetails;
using FitLog.Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using MockQueryable.Moq;
using Ardalis.GuardClauses;
using System.Linq.Expressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class GetCoachProfileDetailsQueryHandlerTests
{
    private readonly Mock<IApplicationDbContext> _contextMock;
    private readonly IMapper _mapper;
    private readonly GetCoachProfileDetailsQueryHandler _handler;

    public GetCoachProfileDetailsQueryHandlerTests()
    {
        _contextMock = new Mock<IApplicationDbContext>();

        // AutoMapper configuration
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new CoachProfileDetailsDto.Mapping());
            cfg.AddProfile(new ProgramOverviewDto.Mapping());
        });
        _mapper = mapperConfig.CreateMapper();

        _handler = new GetCoachProfileDetailsQueryHandler(_contextMock.Object, _mapper);
    }

    [Fact]
    public async Task Handle_ReturnsCoachProfileDetails()
    {
        // Arrange
        var userId = "user_id";
        var user = new AspNetUser { Id = userId };
        var profile = new FitLog.Domain.Entities.Profile
        {
            UserId = userId,
            Bio = "Test bio",
            ProfilePicture = "profile.jpg",
            MajorAchievements = new List<string> { "Achievement 1", "Achievement 2" },
            GalleryImageLinks = new List<string> { "image1.jpg", "image2.jpg" },
            User = user
        };

        var profiles = new List<FitLog.Domain.Entities.Profile> { profile }.AsQueryable().BuildMockDbSet();
        var profilesWithIncludes = new List<FitLog.Domain.Entities.Profile> { profile }.AsQueryable().BuildMockDbSet();

        var programs = new List<Program>
    {
        new Program
        {
            UserId = userId,
            ProgramName = "Program Test 1",
            NumberOfWeeks = 12,
            DaysPerWeek = 5,
            ExperienceLevel = "Intermediate",
            GymType = "Home Gym",
            MusclesPriority = "Upper Body"
        },
        new Program
        {
            UserId = userId,
            ProgramName = "Program Test 2",
            NumberOfWeeks = 8,
            DaysPerWeek = 4,
            ExperienceLevel = "Beginner",
            GymType = "Commercial Gym",
            MusclesPriority = "Lower Body"
        }
    }.AsQueryable().BuildMockDbSet();

        _contextMock.Setup(m => m.Profiles).Returns(profiles.Object);
        _contextMock.Setup(m => m.Programs).Returns(programs.Object);

        _contextMock.Setup(m => m.Profiles)
            .Returns(profilesWithIncludes.Object);

        var query = new GetCoachProfileDetailsQuery(userId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.UserId.Should().Be(userId);
        result.Bio.Should().Be("Test bio");
        result.ProfilePicture.Should().Be("profile.jpg");
        result.MajorAchievements.Should().HaveCount(2);
        result.GalleryImageLinks.Should().HaveCount(2);

        result.ProgramsOverview.Should().NotBeNull();
        result.ProgramsOverview.Should().HaveCount(2);

        var program1 = result.ProgramsOverview?.FirstOrDefault(p => p.ProgramName == "Program Test 1");
        program1.Should().NotBeNull();
        program1!.NumberOfWeeks.Should().Be(12);
        program1.DaysPerWeek.Should().Be(5);
        program1.ExperienceLevel.Should().Be("Intermediate");
        program1.GymType.Should().Be("Home Gym");

        var program2 = result.ProgramsOverview?.FirstOrDefault(p => p.ProgramName == "Program Test 2");
        program2.Should().NotBeNull();
        program2!.NumberOfWeeks.Should().Be(8);
        program2.DaysPerWeek.Should().Be(4);
        program2.ExperienceLevel.Should().Be("Beginner");
        program2.GymType.Should().Be("Commercial Gym");
    }


    [Fact]
    public async Task Handle_ThrowsNotFoundException_WhenProfileNotFound()
    {
        // Arrange
        var query = new GetCoachProfileDetailsQuery("non_existing_user_id");
        var userId = "user_id";
        var user = new AspNetUser { Id = userId };
        var profile = new FitLog.Domain.Entities.Profile
        {
            UserId = userId,
            Bio = "Test bio",
            ProfilePicture = "profile.jpg",
            MajorAchievements = new List<string> { "Achievement 1", "Achievement 2" },
            GalleryImageLinks = new List<string> { "image1.jpg", "image2.jpg" },
            User = user
        };
        //var profiles = new List<FitLog.Domain.Entities.Profile>().AsQueryable().BuildMockDbSet();
        var programs = new List<Program>().AsQueryable().BuildMockDbSet();
        var profilesWithIncludes = new List<FitLog.Domain.Entities.Profile> { profile }.AsQueryable().BuildMockDbSet();

        _contextMock.Setup(m => m.Profiles).Returns(profilesWithIncludes.Object);
        _contextMock.Setup(m => m.Programs).Returns(programs.Object);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(query, CancellationToken.None));
    }

}
