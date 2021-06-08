// <copyright file="JwtTokenDto.cs" company="github.com/edu_costa">
// Copyright (c) github.com/edu_costa. All rights reserved.
// </copyright>

namespace Exchange.Domain.Dtos
{
    using System;

    /// <summary>
    /// JWT Token DTO to render authenticate user token.
    /// </summary>
    public class JwtTokenDto
    {
        /// <summary>
        /// Gets the JWT token.
        /// </summary>
        public string Token { get; init; }

        /// <summary>
        /// Gets the expiration date and time.
        /// </summary>
        public DateTime Expiration { get; init; }
    }
}