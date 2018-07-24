using JetBrains.Annotations;
using Lykke.Sdk.Settings;

namespace Lykke.Service.BinanceAdapter.Settings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class AppSettings : BaseAppSettings
    {
        public BinanceAdapterSettings BinanceAdapterService { get; set; }        
    }
}
