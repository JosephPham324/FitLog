using System;
using System.Threading;
using System.Threading.Tasks;
using FitLog.Application.CoachingServices.Commands.CreateCoachingService;
using FitLog.Application.Common.Interfaces;
using FitLog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using System.Linq;
using System.Collections.Generic;
using FitLog.Infrastructure.Data;
using FluentAssertions;

namespace FitLog.Application.UnitTests.Use_Cases.CoachingService.Create
{
    public class CreateCoachingServiceCommandHandlerTests : IDisposable
    {
        private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;

        public CreateCoachingServiceCommandHandlerTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .Options;

            SeedSampleData();
        }

        private void SeedSampleData()
        {
            using (var context = new ApplicationDbContext(_dbContextOptions))
            {
            }
        }

        public void Dispose()
        {
            using (var context = new ApplicationDbContext(_dbContextOptions))
            {
                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async Task Handle_ShouldCreateCoachingService()
        {
            // Arrange
            var command = new CreateCoachingServiceCommand
            {
                ServiceName = "Test Service",
                Description = "Test Description",
                Duration = 60, // Minutes
                Price = 50.5m,
                ServiceAvailability = true,
                AvailabilityAnnouncement = "Available Now"
            };

            using (var context = new ApplicationDbContext(_dbContextOptions))
            {
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    var handler = new CreateCoachingServiceCommandHandler(context);

                    // Act
                    var result = await handler.Handle(command, CancellationToken.None);

                    // Assert
                    result.Should().NotBeNull();
                    result.Success.Should().BeTrue();

                    // Kiểm tra xem dịch vụ đã được lưu vào cơ sở dữ liệu chưa
                    var entity = await context.CoachingServices.FirstOrDefaultAsync(c => c.ServiceName == "Test Service");

                    entity.Should().NotBeNull();
                    entity!.Description.Should().Be("Test Description");
                    entity.Duration.Should().Be(60);
                    entity.Price.Should().Be(50.5m);
                    entity.ServiceAvailability.Should().BeTrue();
                    entity.AvailabilityAnnouncement.Should().Be("Available Now");

                    // Rollback transaction to avoid affecting the actual database
                    await transaction.RollbackAsync();
                }
            }
        }


    }
}
