using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitLog.Application.Common.Interfaces;
using FitLog.Application.Exercises.Queries.GetExercsieTypes;
using FitLog.Domain.Constants;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace FitLog.Application.UnitTests.Use_Cases.Exercises.Queries.GetTypes;
public class GetExercsieTypesQueryHandlerTests
{
    private Mock<IApplicationDbContext> _contextMock;
    private GetExercsieTypesQueryHandler _handler;

    [SetUp]
    public void Setup()
    {
        _contextMock = new Mock<IApplicationDbContext>();
        _handler = new GetExercsieTypesQueryHandler(_contextMock.Object);
    }

    [Test]
    public async Task Handle_ShouldReturnExerciseTypes()
    {
        // Arrange
        var query = new GetExercsieTypesQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(new List<string?>
            {
                ExerciseTypes.WeightResistance,
                ExerciseTypes.Calisthenics,
                ExerciseTypes.Plyometrics,
                ExerciseTypes.LissCardio,
                ExerciseTypes.HitCardio,
                ExerciseTypes.HiitCardio
            });
    }
}



