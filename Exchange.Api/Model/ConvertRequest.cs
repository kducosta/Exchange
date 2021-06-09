// <copyright file="ConvertRequest.cs" company="github.com/edu_costa">
// Copyright (c) github.com/edu_costa. All rights reserved.
// </copyright>

namespace Exchange.Api.Model
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Represents a currency conversion request query parameters.
    /// </summary>
    public class ConvertRequest
    {
        /// <summary>
        /// Gets or sets the origin currency.
        /// </summary>
        [Required]
        public string From { get; set; }

        /// <summary>
        /// Gets or sets the destination currency.
        /// </summary>
        [Required]
        public string To { get; set; }

        /// <summary>
        /// Gets or sets the amount of money to be converted from origin currency to destination currency.
        /// </summary>
        /// <value>1.0 if not set.</value>
        public float Amount { get; set; } = 1f;
    }
}