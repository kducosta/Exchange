// <copyright file="CurrencyConversionDto.cs" company="github.com/edu_costa">
// Copyright (c) github.com/edu_costa. All rights reserved.
// </copyright>

namespace Exchange.Domain.Dtos
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// DTO to render a conversion transaction.
    /// </summary>
    public class CurrencyConversionDto
    {
        /// <summary>
        /// Gets or Sets Conversion Id.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or Sets the id of the user who requested the conversion.
        /// </summary>
        [Required]
        public string UserId { get; set; }

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
        public float OriginAmount { get; set; }

        /// <summary>
        /// Gets the amount of money converted to destination currency.
        /// </summary>
        public float DestinationAmount => this.OriginAmount * this.Rate;

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