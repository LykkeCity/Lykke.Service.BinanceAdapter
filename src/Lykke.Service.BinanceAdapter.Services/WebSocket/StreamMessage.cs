using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Lykke.Service.BinanceAdapter.Services.WebSocket
{
    public sealed class StreamMessage<T>
    {
        [JsonProperty("stream", NullValueHandling = NullValueHandling.Ignore)]
        public string Stream { get; set; }

        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public T Data { get; set; }
    }

    public sealed class DepthUpdate
    {
        [JsonProperty("e", NullValueHandling = NullValueHandling.Ignore)]
        public string DataE { get; set; }

        [JsonProperty("E", NullValueHandling = NullValueHandling.Ignore)]
        public long EventTimeMs { get; set; }

        [JsonProperty("s", NullValueHandling = NullValueHandling.Ignore)]
        public string Asset { get; set; }

        [JsonProperty("U", NullValueHandling = NullValueHandling.Ignore)]
        public long FirstUpdateId { get; set; }

        [JsonProperty("u", NullValueHandling = NullValueHandling.Ignore)]
        public long LastUpdateId { get; set; }

        [JsonProperty("b", NullValueHandling = NullValueHandling.Ignore)]
        public JToken[][] Bids { get; set; }

        [JsonProperty("a", NullValueHandling = NullValueHandling.Ignore)]
        public JToken[][] Asks { get; set; }
    }
}
