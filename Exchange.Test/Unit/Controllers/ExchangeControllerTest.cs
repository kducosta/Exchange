// <copyright file="ExchangeControllerTest.cs" company="github.com/edu_costa">
// Copyright (c) github.com/edu_costa. All rights reserved.
// </copyright>

namespace Exchange.Test.Unit.Controllers
{
    using Exchange.Api.Controllers;
    using Exchange.Api.Model;
    using Exchange.Domain.Dtos;
    using Exchange.Domain.Repositories;
    using Exchange.Services;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using Xunit;

    /// <summary>
    /// Exchange controller unit tests.
    /// </summary>
    public class ExchangeControllerTest
    {
        /// <summary>
        /// Test a currency conversion.
        /// </summary>
        [Fact]
        public async void ConvertCurrencyTest()
        {
            var originCurrency = "EUR";
            var destinationCurrency = "BRL";

            var conversionDto = new CurrencyConversionDto()
            {
                OriginCurrency = originCurrency,
                DestinationCurrency = destinationCurrency,
            };

            var exchangeServiceMock = new Mock<IExchangeService>();
            exchangeServiceMock.Setup(service =>
                    service.Convert(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<float>()))
                .ReturnsAsync(conversionDto);

            var currencyRepositoryMock = new Mock<ICurrencyConversionRepository>();
            currencyRepositoryMock.Setup(repo =>
                    repo.CreateHistory(It.IsAny<CurrencyConversionDto>(), It.IsAny<string>()))
                .ReturnsAsync(conversionDto);

            var controller = new ExchangeController(exchangeServiceMock.Object, currencyRepositoryMock.Object);
            var request = new ConvertRequest()
            {
                From = originCurrency,
                To = destinationCurrency,
            };

            var response = await controller.Convert(request);

            Assert.Equal(conversionDto.OriginCurrency, originCurrency);
            Assert.Equal(conversionDto.DestinationCurrency, destinationCurrency);
        }

        /// <summary>
        /// Test a currency conversion with a invalid currency, should return a Bad Request
        /// </summary>
        [Fact]
        public async void ConvertInvalidCurrencyTest()
        {
            var exchangeServiceMock = new Mock<IExchangeService>();
            exchangeServiceMock.Setup(service =>
                    service.Convert(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<float>()))
                .ThrowsAsync(new ExchangeRatesApiException("Invalid Currency"));
            var controller = new ExchangeController(exchangeServiceMock.Object, null);
            var request = new ConvertRequest()
            {
                From = "EUR",
                To = "US",
            };

            var response = await controller.Convert(request);

            Assert.IsType<BadRequestObjectResult>(response);
        }
    }
}