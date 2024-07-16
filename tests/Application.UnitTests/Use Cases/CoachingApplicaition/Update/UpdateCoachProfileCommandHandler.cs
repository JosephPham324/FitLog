using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FitLog.Application.CoachProfiles.Commands.UpdateCoachProfile;
using FitLog.Application.Common.Models;
using FitLog.Domain.Entities;
using FitLog.Infrastructure.Data;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FitLog.Application.UnitTests.Use_Cases.CoachingApplicaition.Update
{
    public class UpdateCoachProfileCommandHandlerTests
    {
        private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;

        public UpdateCoachProfileCommandHandlerTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .Options;
        }

        [Fact]
        public async Task Handle_WhenProfileNotFound_ShouldCreateNewProfile()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_dbContextOptions))
            {
                var handler = new UpdateCoachProfileCommandHandler(context);

                var command = new UpdateCoachProfileCommand(
                    "user_id",
                    "New Bio",
                    "profile.jpg",
                    new List<string> { "Achievement 1", "Achievement 2" },
                    new List<string> { "image1.jpg", "image2.jpg" }
                );

                // Act
                var result = await handler.Handle(command, CancellationToken.None);

                // Assert
                result.Should().BeEquivalentTo(Result.Successful());

                var profile = await context.Profiles.FirstOrDefaultAsync(p => p.UserId == "user_id");
                profile.Should().NotBeNull();
                profile!.Bio.Should().Be(command.Bio);
                profile.ProfilePicture.Should().Be(command.ProfilePicture);
                profile.MajorAchievements.Should().BeEquivalentTo(command.MajorAchievements);
                profile.GalleryImageLinks.Should().BeEquivalentTo(command.GalleryImageLinks);

                var coach = context.Profiles.FirstOrDefault(p => p.UserId == command.UserId);
                if (coach != null)
                {
                    context.Profiles.Remove(coach);
                    context.SaveChanges();
                }
            }
        }

        [Fact]
        public async Task Handle_WhenProfileExists_ShouldUpdateProfile()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_dbContextOptions))
            {
                var handler = new UpdateCoachProfileCommandHandler(context);

                var existingProfile = new Profile
                {
                    UserId = "user_id",
                    Bio = "Old Bio",
                    ProfilePicture = "old_profile.jpg",
                    MajorAchievements = new List<string> { "Old Achievement" },
                    GalleryImageLinks = new List<string> { "old_image.jpg" }
                };

                await context.Profiles.AddAsync(existingProfile);
                await context.SaveChangesAsync();

                var command = new UpdateCoachProfileCommand(
                    "user_id",
                    "New Bio",
                    "new_profile.jpg",
                    new List<string> { "New Achievement 1", "New Achievement 2" },
                    new List<string> { "new_image1.jpg", "new_image2.jpg" }
                );

                // Act
                var result = await handler.Handle(command, CancellationToken.None);

                // Assert
                result.Should().BeEquivalentTo(Result.Successful());

                var updatedProfile = await context.Profiles.FirstOrDefaultAsync(p => p.UserId == "user_id");
                updatedProfile.Should().NotBeNull();
                updatedProfile!.Bio.Should().Be(command.Bio);
                updatedProfile.ProfilePicture.Should().Be(command.ProfilePicture);
                updatedProfile.MajorAchievements.Should().BeEquivalentTo(command.MajorAchievements);
                updatedProfile.GalleryImageLinks.Should().BeEquivalentTo(command.GalleryImageLinks);

                var coach = context.Profiles.FirstOrDefault(p=>p.UserId == command.UserId);
                if (coach != null)
                {
                    context.Profiles.Remove(coach);
                    context.Profiles.Remove(existingProfile);
                    context.SaveChanges();
                }
            }
        }
    }
}
