using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitLog.Application.Users.Commands.Register;
using FitLog.Domain.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace FitLog.Application.UnitTests.Use_Cases.Users.Register;


    public class RegisterCommandHandlerTests
    {
        private readonly Mock<UserManager<AspNetUser>> _userManagerMock;
        private readonly RegisterCommandHandler _handler;

        public RegisterCommandHandlerTests()
        {
            var store = new Mock<IUserStore<AspNetUser>>();
            var passwordHasher = new Mock<IPasswordHasher<AspNetUser>>();
            var userValidators = new List<IUserValidator<AspNetUser>> { new Mock<IUserValidator<AspNetUser>>().Object };
            var passwordValidators = new List<IPasswordValidator<AspNetUser>> { new Mock<IPasswordValidator<AspNetUser>>().Object };
            var keyNormalizer = new Mock<ILookupNormalizer>();
            var errors = new Mock<IdentityErrorDescriber>();
            var services = new Mock<IServiceProvider>();
            var logger = new Mock<ILogger<UserManager<AspNetUser>>>();
            var identityOptions = new IdentityOptions();
            var options = Options.Create(identityOptions);

            _userManagerMock = new Mock<UserManager<AspNetUser>>(
                store.Object, options, passwordHasher.Object, userValidators, passwordValidators, keyNormalizer.Object, errors.Object, services.Object, logger.Object);

            _handler = new RegisterCommandHandler(_userManagerMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Return_Success_Result_When_Registration_Succeeds()
        {
            // Arrange
            var command = new RegisterCommand
            {
                Email = "test@example.com",
                Password = "Test@123",
                UserName = "TestUser"
            };

            _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<AspNetUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Success.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_Should_Return_Failure_Result_When_Registration_Fails()
        {
            // Arrange
            var command = new RegisterCommand
            {
                Email = "test@example.com",
                Password = "Test@123",
                UserName = "TestUser"
            };

            var identityErrors = new List<IdentityError> { new IdentityError { Description = "An error occurred" } };
            var identityResult = IdentityResult.Failed(identityErrors.ToArray());

            _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<AspNetUser>(), It.IsAny<string>()))
                .ReturnsAsync(identityResult);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Success.Should().BeFalse();
            result.Errors.Should().Contain("An error occurred");
        }
    }




