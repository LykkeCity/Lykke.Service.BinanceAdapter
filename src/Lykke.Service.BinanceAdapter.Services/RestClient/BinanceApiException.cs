using System;

namespace Lykke.Service.BinanceAdapter.Services.RestClient
{
    public class BinanceApiException : Exception
    {
        public BinanceApiException(int code, string message) : base($"[{code}]: {message}")
        {
            Code = code;
        }

        public int Code { get; }
    }
}
