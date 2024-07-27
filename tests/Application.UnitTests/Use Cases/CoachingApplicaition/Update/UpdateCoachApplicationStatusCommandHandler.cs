//using System;
//using System.Threading;
//using System.Threading.Tasks;
//using Ardalis.GuardClauses;
//using FitLog.Application.CoachProfiles.Commands.UpdateCoachApplicationStatus;
//using FitLog.Application.Common.Interfaces;
//using FitLog.Application.Common.Models;
//using FitLog.Domain.Entities;
//using FitLog.Infrastructure.Data;
//using FluentAssertions;
//using Microsoft.AspNetCore.SignalR;
//using Microsoft.EntityFrameworkCore;
//using Moq;
//using Xunit;

//namespace FitLog.Application.UnitTests.Use_Cases.CoachingApplication.Update
//{
//    public class UpdateCoachApplicationStatusCommandHandlerTests
//    {
//        private readonly Mock<IEmailService> _mockEmailService;
//        private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;
//        private readonly ApplicationDbContext _context;
//        private readonly UpdateCoachApplicationStatusCommandHandler _handler;

//        public UpdateCoachApplicationStatusCommandHandlerTests()
//        {
//            _mockEmailService = new Mock<IEmailService>();

//            _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
//                .UseInMemoryDatabase(databaseName: "TestDatabase") // Use in-memory database for testing
//                .Options;

//            _context = new ApplicationDbContext(_dbContextOptions);

//            var notificationService = new Mock<INotificationService>().Object;

//            _handler = new UpdateCoachApplicationStatusCommandHandler(_context, _mockEmailService.Object, notificationService);
//        }

//        [Fact]
//        public async Task Handle_GivenInvalidApplicationId_ShouldThrowNotFoundException()
//        {
//            // Arrange
//            var command = new UpdateCoachApplicationStatusCommand
//            {
//                ApplicationId = 0, // non-existing id
//                Status = "Approved",
//                UpdatedById = "admin"
//            };

//            // Act
//            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

//            // Assert
//            await act.Should().ThrowAsync<NotFoundException>();
//        }

//        [Fact]
//        public async Task Handle_GivenValidCommand_ShouldUpdateStatusAndSendEmail()
//        {
//            var applicant = await GetOrCreateUserAsync("user_id", "TestUser", "testuser@example.com");
//            var coachApplication = new CoachApplication
//            {
//                ApplicantId = "user_id",
//                Applicant = applicant,
//                Status = "Pending"
//            };

//            var updateApplicant = await GetOrCreateUserAsync("update_id", "updateTest", "testuser@example.com");

//            _context.CoachApplications.Add(coachApplication);
//            await _context.SaveChangesAsync();

//            var applicationId = coachApplication.Id;

//            var command = new UpdateCoachApplicationStatusCommand
//            {
//                ApplicationId = applicationId,
//                Status = "Approved",
//                StatusReason = "All criteria met",
//                UpdatedById = "update_id"
//            };

//            _mockEmailService.Setup(x => x.SendAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
//                .Returns(Task.CompletedTask)
//                .Verifiable();

//            // Act
//            var result = await _handler.Handle(command, CancellationToken.None);

//            // Assert
//            result.Should().BeEquivalentTo(Result.Successful());
//            var updatedApplication = await _context.CoachApplications.FindAsync(command.ApplicationId);
//            updatedApplication!.Status.Should().Be(command.Status);
//            updatedApplication.StatusReason.Should().Be(command.StatusReason);
//            updatedApplication.LastModifiedBy.Should().Be(command.UpdatedById);

//            _mockEmailService.Verify(x => x.SendAsync(
//                "testuser@example.com",
//                "Your Coach Application Status Update",
//                "Dear TestUser,\n\nYour coach application status has been updated to: Approved.\n\nReason: All criteria met\n\nBest regards,\nThe FitLog Team"),
//                Times.Once);

//            var finding = await _context.CoachApplications.FindAsync(applicationId);
//            _context.CoachApplications.Remove(finding!);
//            _context.SaveChanges();
//        }

//        private async Task<AspNetUser> GetOrCreateUserAsync(string userId, string userName, string email)
//        {
//            var existingUser = await _context.AspNetUsers.FirstOrDefaultAsync(u => u.Id == userId);

//            if (existingUser == null)
//            {
//                var newUser = new AspNetUser
//                {
//                    Id = userId,
//                    UserName = userName,
//                    Email = email
//                };

//                _context.AspNetUsers.Add(newUser);
//                await _context.SaveChangesAsync();

//                return newUser;
//            }

//            return existingUser;
//        }
//    }
//}
