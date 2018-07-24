using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Lykke.Service.BinanceAdapter.Services.RestClient.Contracts
{
    public sealed class ExchangeInfo
    {
        [JsonProperty("timezone", NullValueHandling = NullValueHandling.Ignore)]
        public string Timezone { get; set; }

        [JsonProperty("serverTime", NullValueHandling = NullValueHandling.Ignore)]
        public long ServerTime { get; set; }

        [JsonProperty("rateLimits", NullValueHandling = NullValueHandling.Ignore)]
        public RateLimit[] RateLimits { get; set; }

        [JsonProperty("symbols", NullValueHandling = NullValueHandling.Ignore)]
        public Symbol[] Symbols { get; set; }

        [JsonIgnore]
        public IEnumerable<string> Assets => Symbols.Select(x => $"{x.BaseAsset}{x.QuoteAsset}");
    }
}
