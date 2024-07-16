using Ardalis.GuardClauses;
using FitLog.Application.CoachingServices.Commands.DeleteCoachingService;
using FitLog.Application.Common.Exceptions;
using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Models;
using FitLog.Domain.Entities;
using FitLog.Infrastructure.Data;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace FitLog.Application.UnitTests.Use_Cases.CoachingService.Delete
{
    public class DeleteCoachingServiceCommandHandlerTests : IDisposable
    {
        private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;

        public DeleteCoachingServiceCommandHandlerTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .Options;

            SeedSampleData();
        }

        private void SeedSampleData()
        {
            using (var context = new ApplicationDbContext(_dbContextOptions))
            {
                // Seed sample data for testing
                context.CoachingServices.Add(new Domain.Entities.CoachingService
                {
                    ServiceName = "Test Service",
                    Description = "Test Description",
                    Duration = 60,
                    Price = 50.5m,
                    ServiceAvailability = true,
                    AvailabilityAnnouncement = "Available Now",
                    Created = DateTimeOffset.Now,
                    LastModified = DateTimeOffset.Now
                });

                context.SaveChanges();
            }
        }

        public async void Dispose()
        {
            using (var context = new ApplicationDbContext(_dbContextOptions))
            {
                var service = await context.CoachingServices.SingleOrDefaultAsync(c=>c.ServiceName== "Test Service");
                if (service != null)
                {
                     context.CoachingServices.Remove(service);
                    await context.SaveChangesAsync();
                }
            }
        }

        [Fact]
        public async Task Handle_ShouldDeleteCoachingService()
        {
            // Arrange
            

            using (var context = new ApplicationDbContext(_dbContextOptions))
            {
                var service = await context.CoachingServices.SingleOrDefaultAsync(c => c.ServiceName == "Test Service");
                int idservice = 0;
                if (service != null)
                {
                    idservice = service.Id;
                }
                var command = new DeleteCoachingServiceCommand { Id = idservice };
                var handler = new DeleteCoachingServiceCommandHandler(context);

                // Act
                var result = await handler.Handle(command, CancellationToken.None);

                // Assert
                result.Should().NotBeNull();
                result.Success.Should().BeTrue();

                var deletedEntity = await context.CoachingServices.FirstOrDefaultAsync(cs => cs.Id == idservice);

                deletedEntity.Should().BeNull();
            }
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException()
        {
            // Arrange
            var command = new DeleteCoachingServiceCommand { Id = 0 }; // Assuming an ID that does not exist

            using (var context = new ApplicationDbContext(_dbContextOptions))
            {
                var handler = new DeleteCoachingServiceCommandHandler(context);

                // Act & Assert
                await Assert.ThrowsAsync<NotFoundException>(async () =>
                {
                    await handler.Handle(command, CancellationToken.None);
                });
            }
        }
    }

}
