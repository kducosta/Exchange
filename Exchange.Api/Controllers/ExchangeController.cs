// <copyright file="ExchangeController.cs" company="github.com/edu_costa">
// Copyright (c) github.com/edu_costa. All rights reserved.
// </copyright>

namespace Exchange.Api.Controllers
{
    using System.Threading.Tasks;
    using Exchange.Api.Model;
    using Exchange.Domain.Dtos;
    using Exchange.Domain.Repositories;
    using Exchange.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Represents currency exchange endpoint controller.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("/api/v1/[controller]")]
    public class ExchangeController : Controller
    {
        private readonly ExchangeRatesApiService exchangeService;
        private readonly CurrencyConversionRepository currencyConversionRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExchangeController"/> class.
        /// </summary>
        /// <param name="exchangeService">The <see cref="ExchangeRatesApiService"/>.</param>
        /// <param name="currencyConversionRepository">The <see cref="currencyConversionRepository"/>.</param>
        public ExchangeController(ExchangeRatesApiService exchangeService, CurrencyConversionRepository currencyConversionRepository)
        {
            this.exchangeService = exchangeService;
            this.currencyConversionRepository = currencyConversionRepository;
        }

        /// <summary>
        /// Converts a amount of money from a currency to another.
        /// </summary>
        /// <param name="request">The query params/>.</param>
        /// <returns>A <see cref="CurrencyConversionDto"/> with the converted amount.</returns>
        [HttpGet]
        public async Task<ActionResult<CurrencyConversionDto>> Index([FromQuery] ConvertRequest request)
        {
            CurrencyConversionDto conversion;

            try
            {
                conversion = await this.exchangeService.Convert(request.From, request.To, request.Amount);
            }
            catch (ExchangeRatesApiException e)
            {
                if (e.StatusCode != 0)
                {
                    return this.StatusCode((int)e.StatusCode, e.Message);
                }

                return this.BadRequest(e.Message);
            }

            return await this.currencyConversionRepository.CreateHistory(conversion, this.User.Identity?.Name);
        }
    }
}