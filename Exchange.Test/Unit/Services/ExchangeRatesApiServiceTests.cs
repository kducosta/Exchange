// <copyright file="ExchangeRatesApiServiceTests.cs" company="github.com/edu_costa">
// Copyright (c) github.com/edu_costa. All rights reserved.
// </copyright>

namespace Exchange.Test.Unit.Services
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Text.Json;
    using Exchange.Services;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Moq.Contrib.HttpClient;
    using Xunit;

    /// <summary>
    /// Exchange Rate Api Service consumer unit tests.
    /// </summary>
    public class ExchangeRatesApiServiceTests
    {
        private const string InvalidToken = "invalid_token";
        private const string ValidToken = "valid_token";

        private readonly IHttpClientFactory factory;
        private readonly ILogger<ExchangeRatesApiService> serviceLogger;
        private readonly ExchangeRateApiResponse apiResponse;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExchangeRatesApiServiceTests"/> class.
        /// </summary>
        public ExchangeRatesApiServiceTests()
        {
            this.apiResponse = new ()
            {
                Success = true,
                Base = "EUR",
                Rates = new Dictionary<string, float>()
                {
                    { "BRL", 6.17f },
                    { "USD", 1.22f },
                    { "JPY", 133.54f },
                },
            };

            this.serviceLogger = Mock.Of<ILogger<ExchangeRatesApiService>>();

            var handler = new Mock<HttpMessageHandler>();
            handler.SetupRequest(this.GetServiceURL(null))
                .ReturnsResponse(HttpStatusCode.Unauthorized);

            handler.SetupRequest(this.GetServiceURL(InvalidToken))
                .ReturnsResponse(HttpStatusCode.Unauthorized);

            var serializerOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            handler.SetupRequest(this.GetServiceURL(ValidToken))
                .ReturnsResponse(HttpStatusCode.OK, JsonSerializer.Serialize(this.apiResponse, serializerOptions));

            this.factory = handler.CreateClientFactory();
        }

        /// <summary>
        /// Service should throws <see cref="ExchangeRatesApiException"/> when no access key.
        /// </summary>
        [Fact]
        public async void ConvertMissingAccessTokenTest()
        {
            var service = new ExchangeRatesApiService(this.serviceLogger, this.factory, this.CreateConfiguration(null));

            await Assert.ThrowsAsync<ExchangeRatesApiException>(() =>
                service.Convert("TEST", "TEST", 1));
        }

        /// <summary>
        /// Service should throws <see cref="ExchangeRatesApiException"/> with Status Code Unauthorized when invalid access key.
        /// </summary>
        [Fact]
        public async void ConvertInvalidAccessTokenTest()
        {
            var service = new ExchangeRatesApiService(this.serviceLogger, this.factory, this.CreateConfiguration(InvalidToken));

            var exception = await Assert.ThrowsAsync<ExchangeRatesApiException>(() =>
                service.Convert("TEST", "TEST", 1));

            Assert.Equal(HttpStatusCode.Unauthorized, exception.StatusCode);
        }

        /// <summary>
        /// Service should throws <see cref="ExchangeRatesApiException"/> with Status Code Unauthorized when invalid access key.
        /// </summary>
        [Fact]
        public async void ConvertToBaseCurrencyTest()
        {
            var service = this.CreateValidService();

            var originCurrency = "BRL";
            var amount = 10f;
            var response = await service.Convert(originCurrency, "EUR", amount);

            var expectedRate = this.apiResponse.Rates[originCurrency];
            Assert.Equal(expectedRate, response.Rate);
            Assert.Equal(expectedRate * amount, response.DestinationAmount);
        }

        /// <summary>
        /// Service should throws <see cref="ExchangeRatesApiException"/> with Status Code Unauthorized when invalid access key.
        /// </summary>
        [Fact]
        public async void ConvertFromBaseCurrencyTest()
        {
            var service = this.CreateValidService();

            var destinationCurrency = "BRL";
            var amount = 10f;
            var response = await service.Convert("EUR", destinationCurrency, amount);

            var expectedRate = 1f / this.apiResponse.Rates[destinationCurrency];
            Assert.Equal(expectedRate, response.Rate);
            Assert.Equal(expectedRate * amount, response.DestinationAmount);
        }

        private string GetServiceURL(string accessKey)
        {
            return $"http://api.exchangeratesapi.io/v1/latest?access_key={accessKey}&base=eur";
        }

        private IConfiguration CreateConfiguration(string accessKey)
        {
            var configurationDictionary = new Dictionary<string, string>()
            {
                { "ExchangeRatesApi:AccessKey", accessKey },
            };

            return new ConfigurationBuilder()
                .AddInMemoryCollection(configurationDictionary)
                .Build();
        }

        private ExchangeRatesApiService CreateValidService()
        {
            return new (this.serviceLogger, this.factory, this.CreateConfiguration(ValidToken));
        }
    }
}