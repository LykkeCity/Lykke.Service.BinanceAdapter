using Newtonsoft.Json;

namespace Lykke.Service.BinanceAdapter.Services.RestClient.Contracts
{
    public sealed class RateLimit
    {
        [JsonProperty("rateLimitType", NullValueHandling = NullValueHandling.Ignore)]
        public string RateLimitType { get; set; }

        [JsonProperty("interval", NullValueHandling = NullValueHandling.Ignore)]
        public string Interval { get; set; }

        [JsonProperty("limit", NullValueHandling = NullValueHandling.Ignore)]
        public long? Limit { get; set; }
    }
}
