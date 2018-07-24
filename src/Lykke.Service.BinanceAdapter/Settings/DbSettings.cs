using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.BinanceAdapter.Settings
{
    public class DbSettings
    {
        [AzureTableCheck]
        public string LogsConnString { get; set; }
    }
}
