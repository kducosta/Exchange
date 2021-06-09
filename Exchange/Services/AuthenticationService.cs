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
            this.userManager = userManager;
            this.logger = logger;
            this.JwtSecret = configuration["JWT:Secret"] ??
                             throw new ArgumentNullException(nameof(configuration), "No JWT Secret defined");
            this.ValidIssuer = configuration["JWT:ValidIssuer"] ??
                               throw new ArgumentNullException(nameof(configuration), "No JWT Secret defined");
            this.ValidAudience = configuration["JWT:ValidAudience"] ??
                                 throw new ArgumentNullException(nameof(configuration), "No JWT Secret defined");
        }

        private string JwtSecret { get; }

        private string ValidIssuer { get; }

        private string ValidAudience { get; }

        /// <summary>
        /// Authenticate user in service.
        /// </summary>
        /// <param name="loginDto">The <see cref="LoginDto"/>.</param>
        /// <returns>The user JWT or null if wrong inputs.</returns>
        public async Task<JwtTokenDto> Authenticate([NotNull] LoginDto loginDto)
        {
            JwtTokenDto token = null;

            var user = await this.userManager.FindByNameAsync(loginDto.UserName);

            if (user != null && await this.userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                var authClaims = new List<Claim>
                {
                    new (ClaimTypes.Name, user.UserName),
                    new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                var jwtSecret = Encoding.UTF8.GetBytes(this.JwtSecret);
                var authSigninKey = new SymmetricSecurityKey(jwtSecret);

                var jwtToken = new JwtSecurityToken(
                    this.ValidIssuer,
                    this.ValidAudience,
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