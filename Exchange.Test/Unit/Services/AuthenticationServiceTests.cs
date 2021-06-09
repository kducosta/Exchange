// <copyright file="AuthenticationServiceTests.cs" company="github.com/edu_costa">
// Copyright (c) github.com/edu_costa. All rights reserved.
// </copyright>

namespace Exchange.Test.Unit.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Exchange.Domain.Dtos;
    using Exchange.Domain.Models;
    using Exchange.Services;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    /// <summary>
    /// Unit Test for Authentication Service.
    /// </summary>
    public class AuthenticationServiceTests
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<AuthenticationService> serviceLogger;
        private readonly LoginDto dto;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationServiceTests"/> class.
        /// </summary>
        public AuthenticationServiceTests()
        {
            var configurationDictionary = new Dictionary<string, string>()
            {
                { "JWT:Secret", "qwertyuiopasdfghjklzxcvbnm123456" },
                { "JWT:ValidIssuer", "issuer" },
                { "JWT:ValidAudience", "audience" },
            };

            this.configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(configurationDictionary)
                .Build();

            this.serviceLogger = Mock.Of<ILogger<AuthenticationService>>();

            this.dto = new LoginDto()
            {
                UserName = "test",
                Password = "test",
            };
        }

        /// <summary>
        /// Test Authenticate Method with wrong username. Service must return null DTO when password is wrong.
        /// </summary>
        [Fact]
        public async void AuthenticateInvalidUsernameTest()
        {
            var userManagerMock = CreateUserManagerMock();
            userManagerMock.Setup(ur => ur.FindByNameAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<ApplicationUser>(null));

            var service = new AuthenticationService(this.configuration, this.serviceLogger, userManagerMock.Object);
            var tokenDto = await service.Authenticate(this.dto);

            Assert.Null(tokenDto);
        }

        /// <summary>
        /// Test Authenticate Method with wrong password. Service must return null DTO when password is wrong.
        /// </summary>
        [Fact]
        public async void AuthenticateInvalidPasswordTest()
        {
            var userManagerMock = CreateUserManagerMock();
            userManagerMock.Setup(ur => ur.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new ApplicationUser() { UserName = "test" });
            userManagerMock.Setup(ur => ur.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(false);

            var service = new AuthenticationService(this.configuration, this.serviceLogger, userManagerMock.Object);
            var tokenDto = await service.Authenticate(this.dto);

            Assert.Null(tokenDto);
        }

        /// <summary>
        /// Test Authenticate Method to valid data. Service must return a DTO with a not empty token.
        /// </summary>
        [Fact]
        public async void AuthenticateGenerateTokenTest()
        {
            var userManagerMock = CreateUserManagerMock();
            userManagerMock.Setup(ur => ur.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new ApplicationUser() { UserName = "test" });
            userManagerMock.Setup(ur => ur.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            var service = new AuthenticationService(this.configuration, this.serviceLogger, userManagerMock.Object);
            var tokenDto = await service.Authenticate(this.dto);

            Assert.NotNull(tokenDto);
            Assert.False(string.IsNullOrWhiteSpace(tokenDto.Token));
        }

        private static Mock<UserManager<ApplicationUser>> CreateUserManagerMock()
        {
            var store = new Mock<IUserStore<ApplicationUser>>();
            var userManagerMock = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
            userManagerMock.Object.UserValidators.Add(new UserValidator<ApplicationUser>());
            userManagerMock.Object.PasswordValidators.Add(new PasswordValidator<ApplicationUser>());

            return userManagerMock;
        }
    }
}