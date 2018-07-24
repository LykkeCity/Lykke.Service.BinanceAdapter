using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Lykke.Common.ExchangeAdapter.Tools;

namespace Lykke.Service.BinanceAdapter.Services.RestClient
{
    public class PreventTooManyRequestsDelegatingHandler : DelegatingHandler
    {
        private readonly TimeSpanSemaphore _semaphoreSlim;

        public PreventTooManyRequestsDelegatingHandler(int requestsPerSecond, HttpMessageHandler next)
            :base(next)
        {
            _semaphoreSlim = new TimeSpanSemaphore(
                requestsPerSecond,
                TimeSpan.FromSeconds(1));
        }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            return _semaphoreSlim.Run(() => base.SendAsync(request, cancellationToken));
        }
    }
}
