// <copyright file="CurrencyConversion.cs" company="github.com/edu_costa">
// Copyright (c) github.com/edu_costa. All rights reserved.
// </copyright>

namespace Exchange.Domain.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Represent a currency conversion transaction.
    /// </summary>
    public class CurrencyConversion
    {
        /// <summary>
        /// Gets or Sets Conversion Id.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or Sets the user who requested the conversion.
        /// </summary>
        [Required]
        public ApplicationUser User { get; set; }

        /// <summary>
        /// Gets or Sets the origin currency.
        /// </summary>
        [Required]
        public string OriginCurrency { get; set; }

        /// <summary>
        /// Gets or Sets the destination currency.
        /// </summary>
        [Required]
        public string DestinationCurrency { get; set; }

        /// <summary>
        /// Gets or Sets the amount of money to convert.
        /// </summary>
        [Required]
        public float Amount { get; set; }

        /// <summary>
        /// Gets or Sets the rate used in conversion.
        /// </summary>
        [Required]
        public float Rate { get; set; }

        /// <summary>
        /// Gets or Sets when the conversion happened.
        /// </summary>
        [Required]
        public DateTime ConversionTime { get; set; }
    }
}