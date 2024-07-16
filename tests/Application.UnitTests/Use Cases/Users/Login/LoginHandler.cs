using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FitLog.Application.Users.Queries.Login;
using FitLog.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FitLog.Application.UnitTests.Users.Queries.Login
{
    public class LoginHandlerTests
    {
        private readonly Mock<UserManager<AspNetUser>> _userManagerMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly LoginHandler _handler;

        public LoginHandlerTests()
        {
            var store = new Mock<IUserStore<AspNetUser>>();
            _userManagerMock = new Mock<UserManager<AspNetUser>>(
                store.Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<AspNetUser>>().Object,
                new IUserValidator<AspNetUser>[0],
                new IPasswordValidator<AspNetUser>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<AspNetUser>>>().Object);

            _configurationMock = new Mock<IConfiguration>();
            _configurationMock.SetupGet(x => x["Jwt:Issuer"]).Returns("test_issuer");
            _configurationMock.SetupGet(x => x["Jwt:Audience"]).Returns("test_audience");
            _configurationMock.SetupGet(x => x["Jwt:Key"]).Returns("this_is_a_very_secure_key_123456789012345678901234567890123456789012345678901234567890");


            _handler = new LoginHandler(_userManagerMock.Object, _configurationMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Return_Success_Result_With_Token_When_Login_Succeeds()
        {
            // Arrange
            var user = new AspNetUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "testuser",
                Email = "test@example.com"
            };

            var command = new LoginQuery
            {
                Username = "testuser",
                Password = "Test@123"
            };

            _userManagerMock.Setup(x => x.FindByNameAsync(command.Username))
                .ReturnsAsync(user);
            _userManagerMock.Setup(x => x.CheckPasswordAsync(user, command.Password))
                .ReturnsAsync(true);
            _userManagerMock.Setup(x => x.GetRolesAsync(user))
                .ReturnsAsync(new List<string>());

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Success.Should().BeTrue();
            result.Token.Should().NotBeEmpty();
        }

        [Fact]
        public async Task Handle_Should_Return_Failure_Result_When_Login_Fails()
        {
            // Arrange
            var command = new LoginQuery
            {
                Username = "testuser",
                Password = "Test@123"
            };

            _userManagerMock.Setup(x => x.FindByNameAsync(command.Username))
                .ReturnsAsync((AspNetUser?)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Success.Should().BeFalse();
            result.Token.Should().BeEmpty();
        }
    }
}
