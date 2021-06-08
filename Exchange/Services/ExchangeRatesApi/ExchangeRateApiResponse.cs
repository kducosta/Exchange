namespace Exchange.Services
{
    using System.Collections.Generic;

    public class ExchangeRateApiResponse
    {
        /// <summary>
        /// Gets if responds a success operations.
        /// </summary>
        public bool Success { get; init; }

        /// <summary>
        /// Gets the base currency code.
        /// </summary>
        public string Base { get; init; }

        /// <summary>
        /// Gets the conversion rates.
        /// </summary>
        public Dictionary<string, float> Rates { get; init; }
    }
}