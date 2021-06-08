// <copyright file="ExchangeDbContext.cs" company="github.com/edu_costa">
// Copyright (c) github.com/edu_costa. All rights reserved.
// </copyright>

namespace Exchange.Infrastructure.Data
{
    using Exchange.Domain.Models;
    using IdentityServer4.EntityFramework.Options;
    using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// Main database context.
    /// </summary>
    public class ExchangeDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExchangeDbContext"/> class.
        /// </summary>
        /// <param name="options">The <see cref="DbContextOptions"/>.</param>
        /// <param name="operationalStoreOptions">The <see cref="IOptions{OperationalStoreOptions}"/>.</param>
        public ExchangeDbContext(
            DbContextOptions<ExchangeDbContext> options,
            IOptions<OperationalStoreOptions> operationalStoreOptions)
            : base(options, operationalStoreOptions)
        {
        }

        /// <summary>
        /// Gets or Sets the Currency Conversions Data Set.
        /// </summary>
        public virtual DbSet<CurrencyConversion> CurrencyConversions { get; set; }
    }
}
