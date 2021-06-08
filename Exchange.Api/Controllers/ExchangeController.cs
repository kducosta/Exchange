using Microsoft.AspNetCore.Authorization;

namespace Exchange.Api.Controllers
{
    using System.Threading.Tasks;
    using Exchange.Api.Model;
    using Exchange.Domain.Dtos;
    using Exchange.Domain.Repositories;
    using Exchange.Services;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    [ApiController]
    [Route("/api/v1/[controller]")]
    public class ExchangeController : Controller
    {
        private readonly ExchangeRatesApiService exchangeService;
        private readonly CurrencyConversionRepository currencyConversionRepository;

        public ExchangeController(ExchangeRatesApiService exchangeService, CurrencyConversionRepository currencyConversionRepository)
        {
            this.exchangeService = exchangeService;
            this.currencyConversionRepository = currencyConversionRepository;
        }

        [HttpGet]
        public async Task<ActionResult<CurrencyConversionDto>> Index([FromQuery] ConvertRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.From) || string.IsNullOrWhiteSpace(request.To))
            {
                return this.BadRequest("Parameters \"from\" and \"to\" must be defined");
            }

            var conversion = await this.exchangeService.Convert(request.From, request.To, request.Amount);

            return await this.currencyConversionRepository.CreateHistory(conversion, this.User.Identity?.Name);
        }
    }
}