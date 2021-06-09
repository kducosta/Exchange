// <copyright file="LoginDto.cs" company="github.com/edu_costa">
// Copyright (c) github.com/edu_costa. All rights reserved.
// </copyright>

namespace Exchange.Domain.Dtos
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The Login DTO to login request.
    /// </summary>
    public class LoginDto
    {
        /// <summary>
        /// Gets or sets the UserName.
        /// </summary>
        [Required]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the Password.
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}