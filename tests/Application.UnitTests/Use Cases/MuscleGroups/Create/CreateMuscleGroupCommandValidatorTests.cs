using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitLog.Application.Common.Interfaces;
using FitLog.Application.MuscleGroups.Commands.CreateMuscleGroup;
using FitLog.Domain.Entities;
using FitLog.Infrastructure.Data;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using Xunit;

namespace FitLog.Application.UnitTests.Use_Cases.MuscleGroups.Create;
public class CreateMuscleGroupCommandValidatorTests
{
    private readonly CreateMuscleGroupCommandHandler _handler;
    private readonly Mock<IApplicationDbContext> _mockDbContext;

    public CreateMuscleGroupCommandValidatorTests()
    {
        _mockDbContext = new Mock<IApplicationDbContext>();
        _handler = new CreateMuscleGroupCommandHandler(_mockDbContext.Object);
    }

    [Fact]
    public async Task Handle_Should_Add_MuscleGroup_To_Context()
    {
        // Arrange
        var command = new CreateMuscleGroupCommand
        {
            MuscleGroupName = "NewName",
            ImageUrl = "http://validurl.com"
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mockDbContext.Verify(m => m.MuscleGroups.Add(It.Is<MuscleGroup>(mg => mg.MuscleGroupName == command.MuscleGroupName && mg.ImageUrl == command.ImageUrl)), Times.Once());
        _mockDbContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
        NUnit.Framework.Assert.True(result.Success);
    }

}



