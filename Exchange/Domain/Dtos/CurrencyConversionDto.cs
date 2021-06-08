// <copyright file="CurrencyConversionDto.cs" company="github.com/edu_costa">
// Copyright (c) github.com/edu_costa. All rights reserved.
// </copyright>

namespace Exchange.Domain.Dtos
{
    using System;

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
        public string UserId { get; set; }

        /// <summary>
        /// Gets or Sets the origin currency.
        /// </summary>
        public string OriginCurrency { get; set; }

        /// <summary>
        /// Gets or Sets the destination currency.
        /// </summary>
        public string DestinationCurrency { get; set; }

        /// <summary>
        /// Gets or Sets the amount of money to convert.
        /// </summary>
        public float OriginAmount { get; set; }

        /// <summary>
        /// Gets or Sets the amount of money converted to destination currency.
        /// </summary>
        public float DestinationAmount { get; set; }

        /// <summary>
        /// Gets or Sets the rate used in conversion.
        /// </summary>
        public float Rate { get; set; }

        /// <summary>
        /// Gets or Sets when the conversion happened.
        /// </summary>
        public DateTime ConversionTime { get; set; }
    }
}