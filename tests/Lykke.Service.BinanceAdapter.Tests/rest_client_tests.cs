using System.Threading.Tasks;
using Lykke.Logs;
using Lykke.Logs.Loggers.LykkeConsole;
using Lykke.Service.BinanceAdapter.Services.RestClient;
using NUnit.Framework;

namespace Lykke.Service.BinanceAdapter.Tests
{
    public sealed class rest_client_tests
    {
        private static readonly RestClient _client;

        static rest_client_tests()
        {
            var lf = LogFactory.Create().AddUnbufferedConsole();
            _client = new RestClient(lf, 100);
        }

        [Test]
        public async Task get_instruments()
        {
            var assets = await _client.GetExchangeInfo();
            Assert.IsNotEmpty(assets.Symbols);
        }

        [Test]
        public async Task get_orderbook()
        {
            var ob = await _client.GetOrderBook("ETHBTC");

            Assert.IsNotEmpty(ob.Asks);
            Assert.IsNotEmpty(ob.Bids);
        }
    }
}
