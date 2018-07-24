using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.BinanceAdapter.Client 
{
    /// <summary>
    /// BinanceAdapter client settings.
    /// </summary>
    public class BinanceAdapterServiceClientSettings 
    {
        /// <summary>Service url.</summary>
        [HttpCheck("api/isalive")]
        public string ServiceUrl {get; set;}
    }
}
