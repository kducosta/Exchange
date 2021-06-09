// <copyright file="IAuthenticationService.cs" company="github.com/edu_costa">
// Copyright (c) github.com/edu_costa. All rights reserved.
// </copyright>

namespace Exchange.Services
{
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Exchange.Domain.Dtos;

    /// <summary>
    /// Authentication service.
    /// </summary>
    public interface IAuthenticationService
    {
        /// <summary>
        /// Authenticate user in service.
        /// </summary>
        /// <param name="loginDto">The <see cref="LoginDto"/>.</param>
        /// <returns>The user JWT or null if wrong inputs.</returns>
        Task<JwtTokenDto> Authenticate([NotNull] LoginDto loginDto);
    }
}