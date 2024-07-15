using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using FitLog.Application.CoachingServices.Commands.UpdateCoachingService;
using FitLog.Infrastructure.Data;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FitLog.Application.UnitTests.Use_Cases.CoachingService.Update;
public class UpdateCoachingServiceCommandHandlerTests : IDisposable
{
    private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;
    private readonly ApplicationDbContext _context;

    public UpdateCoachingServiceCommandHandlerTests()
    {
        _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .Options;

        _context = new ApplicationDbContext(_dbContextOptions);

        SeedSampleData();
    }

    private void SeedSampleData()
    {
        // Seed sample data for testing
        _context.CoachingServices.Add(new Domain.Entities.CoachingService
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

        _context.SaveChanges();
    }

    public async void Dispose()
    {
        var service = await _context.CoachingServices.SingleOrDefaultAsync(s => s.ServiceName == "Test Service");
        if(service != null)
        {
             _context.CoachingServices.Remove(service);
            await _context.SaveChangesAsync();
        }
        _context.Dispose();
    }

    [Fact]
    public async Task Handle_ShouldUpdateCoachingService()
    {
        // Arrange
        var service = await _context.CoachingServices.SingleOrDefaultAsync(s=>s.ServiceName=="Test Service");
        Assert.NotNull(service);

        var command = new UpdateCoachingServiceCommand
        {
            Id = service.Id,
            ServiceName = "Updated Service",
            Description = "Updated Description",
            Duration = 90,
            Price = 75.5m,
            ServiceAvailability = false,
            AvailabilityAnnouncement = "Unavailable Now"
        };

        var handler = new UpdateCoachingServiceCommandHandler(_context);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();

        // Verify if the service has been updated in the database
        var updatedEntity = await _context.CoachingServices.FirstOrDefaultAsync(cs => cs.Id == service.Id);

        updatedEntity.Should().NotBeNull();
        updatedEntity!.ServiceName.Should().Be("Updated Service");
        updatedEntity.Description.Should().Be("Updated Description");
        updatedEntity.Duration.Should().Be(90);
        updatedEntity.Price.Should().Be(75.5m);
        updatedEntity.ServiceAvailability.Should().BeFalse();
        updatedEntity.AvailabilityAnnouncement.Should().Be("Unavailable Now");
        updatedEntity.LastModified.Should().BeCloseTo(DateTimeOffset.Now, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException()
    {
        // Arrange
        var command = new UpdateCoachingServiceCommand
        {
            Id = 999, // Assuming an ID that does not exist
            ServiceName = "Non-existent Service",
            Description = "Non-existent Description",
            Duration = 90,
            Price = 75.5m,
            ServiceAvailability = false,
            AvailabilityAnnouncement = "Unavailable Now"
        };

        var handler = new UpdateCoachingServiceCommandHandler(_context);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(async () =>
        {
            await handler.Handle(command, CancellationToken.None);
        });
    }
}
