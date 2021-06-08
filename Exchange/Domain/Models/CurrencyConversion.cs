namespace Exchange.Domain.Model
{
    using System;
    using Microsoft.AspNetCore.Identity;

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
        public IdentityUser User { get; set; }

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
        public double Amount { get; set; }

        /// <summary>
        /// Gets or Sets the rate used in conversion.
        /// </summary>
        public double Rate { get; set; }

        /// <summary>
        /// Gets or Sets when the conversion happened.
        /// </summary>
        public DateTime ConversionTime { get; set; }
    }
}