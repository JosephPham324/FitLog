using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Models;
using FitLog.Application.Users.Commands.Register;
using FitLog.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FitLog.Application.UnitTests.Users.Commands.Register
{
    public class RegisterCommandHandlerTests
    {
        private readonly Mock<UserManager<AspNetUser>> _userManagerMock;
        private readonly Mock<IEmailService> _emailServiceMock;
        private readonly RegisterCommandHandler _handler;

        public RegisterCommandHandlerTests()
        {
            // Create necessary dependencies for UserManager
            var store = new Mock<IUserStore<AspNetUser>>();
            var options = new Mock<IOptions<IdentityOptions>>();
            var passwordHasher = new Mock<IPasswordHasher<AspNetUser>>();
            var userValidators = new IUserValidator<AspNetUser>[0];
            var passwordValidators = new IPasswordValidator<AspNetUser>[0];
            var keyNormalizer = new Mock<ILookupNormalizer>();
            var errors = new Mock<IdentityErrorDescriber>();
            var services = new Mock<IServiceProvider>();
            var logger = new Mock<ILogger<UserManager<AspNetUser>>>();

            // Initialize UserManager with mock dependencies
            _userManagerMock = new Mock<UserManager<AspNetUser>>(
                store.Object,
                options.Object,
                passwordHasher.Object,
                userValidators,
                passwordValidators,
                keyNormalizer.Object,
                errors.Object,
                services.Object,
                logger.Object);

            _emailServiceMock = new Mock<IEmailService>();

            _handler = new RegisterCommandHandler(_userManagerMock.Object, _emailServiceMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Return_Success_When_Registration_Succeeds()
        {
            // Arrange
            var command = new RegisterCommand
            {
                Email = "test@example.com",
                Password = "Test@123",
                UserName = "testuser",
                PhoneNumber = "+1234567890"
            };

            _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<AspNetUser>(), command.Password))
                .ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(x => x.GenerateEmailConfirmationTokenAsync(It.IsAny<AspNetUser>()))
                .ReturnsAsync("test_token");
            _emailServiceMock.Setup(x => x.SendAsync(command.Email, It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Success.Should().BeTrue();
            _emailServiceMock.Verify(x => x.SendAsync(command.Email, It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Return_Failure_When_Registration_Fails()
        {
            // Arrange
            var command = new RegisterCommand
            {
                Email = "test@example.com",
                Password = "Test@123",
                UserName = "testuser",
                PhoneNumber = "+1234567890"
            };

            var identityErrors = new IdentityError[]
            {
                new IdentityError { Description = "Email already exists." }
            };
            _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<AspNetUser>(), command.Password))
                .ReturnsAsync(IdentityResult.Failed(identityErrors));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Success.Should().BeFalse();
            result.Errors.Should().Contain("Email already exists.");
        }

        [Fact]
        public async Task Handle_Should_Send_Email_Confirmation_When_Registration_Succeeds()
        {
            // Arrange
            var command = new RegisterCommand
            {
                Email = "test@example.com",
                Password = "Test@123",
                UserName = "testuser",
                PhoneNumber = "+1234567890"
            };

            _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<AspNetUser>(), command.Password))
                .ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(x => x.GenerateEmailConfirmationTokenAsync(It.IsAny<AspNetUser>()))
                .ReturnsAsync("test_token");

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _userManagerMock.Verify(x => x.GenerateEmailConfirmationTokenAsync(It.IsAny<AspNetUser>()), Times.Once);
            _emailServiceMock.Verify(x => x.SendAsync(command.Email, "Confirm Your Email", It.Is<string>(body => body.Contains("https://localhost:44447/confirm-email?token=test_token&email=test%40example.com"))), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Return_Failure_When_Username_Is_Not_Unique()
        {
            // Arrange
            var command = new RegisterCommand
            {
                Email = "test@example.com",
                Password = "Test@123",
                UserName = "existinguser",
                PhoneNumber = "+1234567890"
            };

            _userManagerMock.Setup(x => x.FindByNameAsync(command.UserName))
                .ReturnsAsync(new AspNetUser());

            var validator = new RegisterCommandValidator(_userManagerMock.Object);

            // Act
            var validationResult = await validator.ValidateAsync(command, CancellationToken.None);

            // Assert
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().ContainSingle(error => error.PropertyName == "UserName" && error.ErrorMessage == "The specified username already exists.");
        }

        [Fact]
        public async Task Handle_Should_Return_Failure_When_Email_Is_Not_Unique()
        {
            // Arrange
            var command = new RegisterCommand
            {
                Email = "existing@example.com",
                Password = "Test@123",
                UserName = "testuser",
                PhoneNumber = "+1234567890"
            };

            _userManagerMock.Setup(x => x.FindByEmailAsync(command.Email))
                .ReturnsAsync(new AspNetUser());

            var validator = new RegisterCommandValidator(_userManagerMock.Object);

            // Act
            var validationResult = await validator.ValidateAsync(command, CancellationToken.None);

            // Assert
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().ContainSingle(error => error.PropertyName == "Email" && error.ErrorMessage == "The specified email is already in use.");
        }

        [Fact]
        public async Task Handle_Should_Return_Failure_When_Password_Is_Too_Short()
        {
            // Arrange
            var command = new RegisterCommand
            {
                Email = "test@example.com",
                Password = "short",
                UserName = "testuser",
                PhoneNumber = "+1234567890"
            };

            var validator = new RegisterCommandValidator(_userManagerMock.Object);

            // Act
            var validationResult = await validator.ValidateAsync(command);

            // Assert
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().ContainSingle(error => error.PropertyName == "Password" && error.ErrorMessage == "Password must be at least 6 characters long.");
        }

        [Fact]
        public async Task Handle_Should_Return_Failure_When_PhoneNumber_Is_Invalid()
        {
            // Arrange
            var command = new RegisterCommand
            {
                Email = "test@example.com",
                Password = "Test@123",
                UserName = "testuser",
                PhoneNumber = "invalid_phone_number"
            };

            var validator = new RegisterCommandValidator(_userManagerMock.Object);

            // Act
            var validationResult = await validator.ValidateAsync(command);

            // Assert
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().ContainSingle(error => error.PropertyName == "PhoneNumber" && error.ErrorMessage == "Invalid phone number format.");
        }
    }
}
