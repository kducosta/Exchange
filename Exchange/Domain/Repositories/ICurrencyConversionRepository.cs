// <copyright file="ICurrencyConversionRepository.cs" company="github.com/edu_costa">
// Copyright (c) github.com/edu_costa. All rights reserved.
// </copyright>

namespace Exchange.Domain.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Exchange.Domain.Dtos;
    using Exchange.Domain.Models;

    /// <summary>
    /// The Currency Conversion repository interface.
    /// </summary>
    public interface ICurrencyConversionRepository
    {
        /// <summary>
        /// Registry a currency conversion transaction.
        /// </summary>
        /// <param name="conversion">Conversion data.</param>
        /// <param name="username">Requester username.</param>
        /// <returns>The new instance or null if fails.</returns>
        Task<CurrencyConversionDto> CreateHistory(CurrencyConversionDto conversion, string username);

        /// <summary>
        /// Get all conversion transactions made by a user.
        /// </summary>
        /// <param name="userId">The <see cref="ApplicationUser"/> Id.</param>
        /// <returns>List of <see cref="CurrencyConversionDto"/>.</returns>
        Task<List<CurrencyConversionDto>> GetUserConversionHistory(string userId);
    }
}