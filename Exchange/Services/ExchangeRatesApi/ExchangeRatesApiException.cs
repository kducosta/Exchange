namespace Exchange.Services
{
    using System;
    using System.Net;

    public class ExchangeRatesApiException : Exception
    {
        public ExchangeRatesApiException(string message)
            : base(message)
        {
        }

        public ExchangeRatesApiException(string message, Exception e)
            : base(message, e)
        {
        }

        public ExchangeRatesApiException(HttpStatusCode statusCode, string message)
            : base(message)
        {
            this.StatusCodeCode = statusCode;
        }

        public HttpStatusCode StatusCodeCode { get; }

    }
}