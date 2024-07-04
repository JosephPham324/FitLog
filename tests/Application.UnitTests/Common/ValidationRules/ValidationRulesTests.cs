using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.ValidationRules;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Validators;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

[TestFixture]
public class ValidationRulesTests
{
    [Test]
    public void BeAValidUrl_ValidUrl_ShouldReturnTrue()
    {
        // Arrange
        string validUrl = "https://www.example.com";

        // Act
        var result = ValidationRules.BeAValidUrl(validUrl);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public void BeAValidUrl_InvalidUrl_ShouldReturnFalse()
    {
        // Arrange
        string invalidUrl = "invalid_url";

        // Act
        var result = ValidationRules.BeAValidUrl(invalidUrl);

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    public void BeAValidUrl_NullUrl_ShouldReturnFalse()
    {
        // Arrange
        string? nullUrl = null;

        // Act
        var result = ValidationRules.BeAValidUrl(nullUrl);

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    public async Task MustExist_EntityExists_ShouldPassValidation()
    {
        // Arrange
        var mockContext = new Mock<IApplicationDbContext>();
        var entity = new TestEntity { Id = 1 };
        mockContext.Setup(x => x.Set<TestEntity>().Find(It.IsAny<object[]>())).Returns(entity);

        var validator = new TestValidator(mockContext.Object);

        // Act
        var result = await validator.ValidateAsync(new TestClass { EntityId = 1 });

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public async Task MustExist_EntityDoesNotExist_ShouldFailValidation()
    {
        // Arrange
        var mockContext = new Mock<IApplicationDbContext>();
        TestEntity? entity = null;
        mockContext.Setup(x => x.Set<TestEntity>().Find(It.IsAny<object[]>())).Returns(entity);

        var validator = new TestValidator(mockContext.Object);

        // Act
        var result = await validator.ValidateAsync(new TestClass { EntityId = 1 });

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.ErrorMessage == "TestEntity with specified key(s) does not exist.");
    }
}

public class TestClass
{
    public int EntityId { get; set; }
}

public class TestEntity
{
    public int Id { get; set; }
}

public class TestValidator : AbstractValidator<TestClass>
{
    public TestValidator(IApplicationDbContext context)
    {
        RuleFor(x => x.EntityId).MustExist<TestClass, TestEntity, int>(context, x => x.EntityId, nameof(TestEntity));
    }
}
