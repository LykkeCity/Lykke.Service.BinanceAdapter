using System.Collections.Generic;
using Newtonsoft.Json;

namespace Lykke.Service.BinanceAdapter.Services.RestClient.Contracts
{
    public sealed class Symbol
    {
        [JsonProperty("symbol", NullValueHandling = NullValueHandling.Ignore)]
        public string SymbolSymbol { get; set; }

        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        public string Status { get; set; }

        [JsonProperty("baseAsset", NullValueHandling = NullValueHandling.Ignore)]
        public string BaseAsset { get; set; }

        [JsonProperty("baseAssetPrecision", NullValueHandling = NullValueHandling.Ignore)]
        public long? BaseAssetPrecision { get; set; }

        [JsonProperty("quoteAsset", NullValueHandling = NullValueHandling.Ignore)]
        public string QuoteAsset { get; set; }

        [JsonProperty("quotePrecision", NullValueHandling = NullValueHandling.Ignore)]
        public long? QuotePrecision { get; set; }

        [JsonProperty("orderTypes", NullValueHandling = NullValueHandling.Ignore)]
        public string[] OrderTypes { get; set; }

        [JsonProperty("icebergAllowed", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IcebergAllowed { get; set; }

        [JsonProperty("filters", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, string>[] Filters { get; set; }
    }
}
