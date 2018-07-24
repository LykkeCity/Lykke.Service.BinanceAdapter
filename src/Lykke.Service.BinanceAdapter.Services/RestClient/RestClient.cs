using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Lykke.Common.ExchangeAdapter.Server;
using Lykke.Common.Log;
using Lykke.Service.BinanceAdapter.Services.RestClient.Contracts;
using Newtonsoft.Json.Linq;

namespace Lykke.Service.BinanceAdapter.Services.RestClient
{


    public sealed class RestClient
    {
        private readonly HttpClient _client;

        public RestClient(ILogFactory lf, int orderBookDepth)
        {
            var pathsToIgnore = new []
            {
                "/api/v1/depth"
            };

            _orderBookDepth = GetAllowedOrderBookSize(orderBookDepth);
            var requestsPerSecond = GetRequestsPerSecond(_orderBookDepth);

            _client = new HttpClient(
                new LoggingHandler(
                    lf.CreateLog(this),
                    new PreventTooManyRequestsDelegatingHandler(requestsPerSecond, new HttpClientHandler()),
                    pathsToIgnore))
            {
                BaseAddress = new Uri("https://api.binance.com/api/v1/")
            };
        }

        private static int GetRequestsPerSecond(int orderBookDepth)
        {
            int weight;

            switch (orderBookDepth)
            {
                case 1000:
                    weight = 10;
                    break;
                case 500:
                    weight = 5;
                    break;
                default:
                    weight = 1;
                    break;
            }

            return 1200 / weight / 60;
        }

        private static readonly int[] AllowedOrderBookSize = {5, 10, 20, 50, 100, 500, 1000};
        private readonly int _orderBookDepth;

        private static int GetAllowedOrderBookSize(int orderBookDepth)
        {
            var size = AllowedOrderBookSize.FirstOrDefault(x => x >= orderBookDepth);

            if (size == 0) throw new ArgumentOutOfRangeException(
                nameof(orderBookDepth),
                "Maximum allowed order book size is 1000");

            return size;
        }

        public async Task<ExchangeInfo> GetExchangeInfo()
        {
            using (var msg = await _client.GetAsync("exchangeInfo"))
            {
                return await ReadAsBinanceResponse<ExchangeInfo>(msg);
            }
        }

        private static async Task<T> ReadAsBinanceResponse<T>(HttpResponseMessage msg)
        {
            msg.EnsureSuccessStatusCode();

            var json = await msg.Content.ReadAsAsync<JToken>();
            if (json is JObject jObject && jObject.ContainsKey("code") && jObject["code"].Value<int>() != 0)
            {
                throw new BinanceApiException(jObject["code"].Value<int>(), jObject["msg"].Value<string>());
            }

            return json.ToObject<T>();
        }

        public async Task<BinanceOrderBook> GetOrderBook(string asset)
        {
            using (var msg = await _client.GetAsync($"depth?symbol={WebUtility.UrlEncode(asset.ToUpper())}" +
                                                         $"&limit={_orderBookDepth}"))
            {
                return await ReadAsBinanceResponse<BinanceOrderBook>(msg);
            }
        }
    }
}
