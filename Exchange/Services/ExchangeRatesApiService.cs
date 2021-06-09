// <copyright file="ExchangeRatesApiService.cs" company="github.com/edu_costa">
// Copyright (c) github.com/edu_costa. All rights reserved.
// </copyright>

namespace Exchange.Services
{
    using System;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Exchange.Domain.Dtos;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Exchange Rates Api remote service.
    /// </summary>
    public class ExchangeRatesApiService : IExchangeService
    {
        private readonly ILogger<ExchangeRatesApiService> logger;
        private readonly IHttpClientFactory httpClientFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExchangeRatesApiService"/> class.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger{ExchangeRatesApiService}"/>.</param>
        /// <param name="httpClientFactory">The <see cref="IHttpClientFactory"/>.</param>
        /// <param name="configuration">The applications <see cref="IConfiguration"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// Throws when no <see cref="IHttpClientFactory"/> is provided from the Host.
        /// </exception>
        public ExchangeRatesApiService(
            ILogger<ExchangeRatesApiService> logger,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
        {
            this.logger = logger;
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            this.AccessKey = configuration["ExchangeRatesApi:AccessKey"];
        }

        /// <summary>
        /// Gets Exchange Rates Api Access Key.
        /// </summary>
        private string AccessKey { get; }

        /// <inheritdoc />
        /// <exception cref="ExchangeRatesApiException">Throws when at least one of the currencies is invalid.</exception>
        public async Task<CurrencyConversionDto> Convert(string originCurrency, string destinationCurrency, float amount)
        {
            var rates = await this.RequestApiRates();
            if (rates.Base != originCurrency && !rates.Rates.ContainsKey(originCurrency))
            {
                throw new ExchangeRatesApiException($"The currency {originCurrency} is not available in service");
            }

            if (rates.Base != destinationCurrency && !rates.Rates.ContainsKey(destinationCurrency))
            {
                throw new ExchangeRatesApiException($"The currency {destinationCurrency} is not available in service");
            }

            var sourceRate = rates.Base != originCurrency ? rates.Rates[originCurrency] : 1f;
            var targetRate = rates.Base != destinationCurrency ? rates.Rates[destinationCurrency] : 1f;
            var rate = sourceRate / targetRate;

            return new CurrencyConversionDto()
            {
                OriginCurrency = originCurrency,
                DestinationCurrency = destinationCurrency,
                OriginAmount = amount,
                Rate = rate,
                ConversionTime = DateTime.UtcNow,
            };
        }

        /// <summary>
        /// Request current to Exchange Rates API web service.
        /// </summary>
        /// <returns>The <see cref="ExchangeRateApiResponse"/>.</returns>
        /// <exception cref="ExchangeRatesApiException">
        /// Throws when no access key or api response with no success response.
        /// </exception>
        private async Task<ExchangeRateApiResponse> RequestApiRates()
        {
            this.logger.LogInformation("Requesting Rates to Exchange Rates API");

            if (this.AccessKey == null)
            {
                this.logger.LogError("No access key configured");
                throw new ExchangeRatesApiException("No Exchange Rates Api access key provided by configuration");
            }

            var httpClient = this.httpClientFactory.CreateClient();
            var url = $"https://api.exchangeratesapi.io/v1/latest?access_key={this.AccessKey}&base=eur";
            HttpResponseMessage response;

            try
            {
                response = await httpClient.GetAsync(url);
            }
            catch (HttpRequestException e)
            {
                throw new ExchangeRatesApiException(e.StatusCode, "Failed to request Exchange Rates API", e);
            }

            if (!response.IsSuccessStatusCode)
            {
                this.logger.LogError(
                    "Exchange Rates API response with error code {StatusCode}: {Message}",
                    response.StatusCode,
                    response.ReasonPhrase);

                throw new ExchangeRatesApiException(
                    response.StatusCode,
                    $"Error response from Exchange Rates API: {response.ReasonPhrase}");
            }

            var deserializerOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            var json = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<ExchangeRateApiResponse>(json, deserializerOptions);
        }
    }
}