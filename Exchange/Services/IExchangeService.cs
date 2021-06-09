// <copyright file="IExchangeService.cs" company="github.com/edu_costa">
// Copyright (c) github.com/edu_costa. All rights reserved.
// </copyright>

namespace Exchange.Services
{
    using System.Threading.Tasks;
    using Exchange.Domain.Dtos;

    /// <summary>
    /// Currency Exchange Service.
    /// </summary>
    public interface IExchangeService
    {
        /// <summary>
        /// Converts a amount of money from origin to destination currency.
        /// </summary>
        /// <param name="originCurrency">The origin currency.</param>
        /// <param name="destinationCurrency">The destination currency.</param>
        /// <param name="amount">The amount, in origin currency, to be converted.</param>
        /// <returns>A <see cref="CurrencyConversionDto"/> containing the transaction data.</returns>
        Task<CurrencyConversionDto> Convert(string originCurrency, string destinationCurrency, float amount);
    }
}