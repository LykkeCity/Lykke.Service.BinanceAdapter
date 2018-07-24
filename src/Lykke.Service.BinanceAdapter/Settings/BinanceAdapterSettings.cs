using JetBrains.Annotations;
using Lykke.Common.ExchangeAdapter.Server.Settings;
using Lykke.Service.BinanceAdapter.Services;

namespace Lykke.Service.BinanceAdapter.Settings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class BinanceAdapterSettings
    {
        public DbSettings Db { get; set; }
        public OrderBookProcessingSettings OrderBooks { get; set; }
    }
}
