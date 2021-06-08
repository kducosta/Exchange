// <copyright file="AuthenticationService.cs" company="github.com/edu_costa">
// Copyright (c) github.com/edu_costa. All rights reserved.
// </copyright>

namespace Exchange.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;
    using Exchange.Domain.Dtos;
    using Exchange.Domain.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Microsoft.IdentityModel.Tokens;

    /// <summary>
    /// The Authentication Service.
    /// </summary>
    public class AuthenticationService
    {
        private readonly IConfiguration configuration;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<AuthenticationService> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationService"/> class.
        /// </summary>
        /// <param name="configuration">The <see cref="IConfiguration"/>.</param>
        /// <param name="logger">The <see cref="ILogger{AuthenticationService}"/>.</param>
        /// <param name="userManager">The <see cref="UserManager{IdentityUser}"/>.</param>
        public AuthenticationService(IConfiguration configuration, ILogger<AuthenticationService> logger, UserManager<ApplicationUser> userManager)
        {
            this.configuration = configuration;
            this.userManager = userManager;
            this.logger = logger;
        }

        /// <summary>
        /// Authenticate user in service.
        /// </summary>
        /// <param name="loginDto">The <see cref="LoginDto"/>.</param>
        /// <returns>The user JWT or null if wrong inputs.</returns>
        public async Task<JwtTokenDto> Autheticate([NotNull] LoginDto loginDto)
        {
            JwtTokenDto token = null;
            var jwtSecret = this.configuration["JWT:Secret"];

            if (jwtSecret == null)
            {
                this.logger.LogError("No JWT secret defined, users cannot authenticate");
                return null;
            }

            var user = await this.userManager.FindByNameAsync(loginDto.UserName);

            if (user != null && await this.userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                var authClaims = new List<Claim>
                {
                    new (ClaimTypes.Name, user.UserName),
                    new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                var authSigninKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));

                var jwtToken = new JwtSecurityToken(
                    this.configuration["JWT:ValidIssuer"],
                    this.configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigninKey, SecurityAlgorithms.HmacSha256));

                token = new JwtTokenDto()
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                    Expiration = jwtToken.ValidTo,
                };
            }

            return token;
        }
    }
}