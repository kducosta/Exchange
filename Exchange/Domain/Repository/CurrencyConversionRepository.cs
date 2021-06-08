// <copyright file="CurrencyConversionRepository.cs" company="github.com/edu_costa">
// Copyright (c) github.com/edu_costa. All rights reserved.
// </copyright>

namespace Exchange.Domain.Repository
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Exchange.Domain.Dtos;
    using Exchange.Domain.Model;
    using Exchange.Infrastructure.Data;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// The Currency Conversion repository
    /// </summary>
    public class CurrencyConversionRepository
    {
        private readonly ExchangeDbContext dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="CurrencyConversionRepository"/> class.
        /// </summary>
        /// <param name="dbContext">Application datasource context.</param>
        public CurrencyConversionRepository(ExchangeDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <summary>
        /// Registry a currency conversion transaction.
        /// </summary>
        /// <param name="conversion">Conversion data.</param>
        /// <returns>The new instance or null if fails.</returns>
        public async Task<CurrencyConversionDto> CreateHistory(CurrencyConversionDto conversion)
        {
            var user = await this.dbContext.Users.FindAsync(conversion.UserId);

            var newConversion = new CurrencyConversion()
            {
                User = user,
                OriginCurrency = conversion.OriginCurrency,
                DestinationCurrency = conversion.DestinationCurrency,
                Amount = conversion.OriginAmount,
                Rate = conversion.Rate,
                ConversionTime = conversion.ConversionTime,
            };

            await this.dbContext.CurrencyConversions.AddAsync(newConversion);
            var result = await this.dbContext.SaveChangesAsync();

            conversion.Id = conversion.Id;
            return result > 0 ? conversion : null;
        }

        /// <summary>
        /// Get all conversion transactions made by a user.
        /// </summary>
        /// <param name="userId">The <see cref="IdentityUser"/>.</param>
        /// <returns>List of <see cref="CurrencyConversionDto"/>.</returns>
        public async Task<List<CurrencyConversionDto>> GetUserHistory(string userId)
        {
            var user = await this.dbContext.Users.FindAsync(userId);
            return await this.dbContext.CurrencyConversions
                .Select(conversion => new CurrencyConversionDto()
                {
                    Id = conversion.Id,
                    UserId = user.Id,
                    OriginCurrency = conversion.OriginCurrency,
                    DestinationCurrency = conversion.DestinationCurrency,
                    OriginAmount = conversion.Amount,
                    DestinationAmount = conversion.Amount * conversion.Rate,
                    Rate = conversion.Rate,
                    ConversionTime = conversion.ConversionTime,
                }).ToListAsync();
        }
    }
}