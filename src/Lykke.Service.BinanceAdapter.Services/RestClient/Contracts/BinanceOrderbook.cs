using System;
using System.Linq;
using Lykke.Common.ExchangeAdapter.Contracts;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Lykke.Service.BinanceAdapter.Services.RestClient.Contracts
{
    public sealed class BinanceOrderBook
    {
        [JsonProperty("lastUpdateId", NullValueHandling = NullValueHandling.Ignore)]
        public long LastUpdateId { get; set; }

        [JsonProperty("bids", NullValueHandling = NullValueHandling.Ignore)]
        public JToken[][] Bids { get; set; }

        [JsonProperty("asks", NullValueHandling = NullValueHandling.Ignore)]
        public JToken[][] Asks { get; set; }

        public OrderBook ToOrderBook(string asset)
        {
            return new OrderBook("binance", asset,
                DateTime.UtcNow,
                asks: Asks.Select(x => new OrderBookItem(x[0].Value<decimal>(), x[1].Value<decimal>())),
                bids: Bids.Select(x => new OrderBookItem(x[0].Value<decimal>(), x[1].Value<decimal>()))
            );
        }
    }
}
