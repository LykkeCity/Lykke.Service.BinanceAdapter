using JetBrains.Annotations;
using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.BinanceAdapter.Settings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class BinanceAdapterSettings
    {
        public DbSettings Db { get; set; }
    }
}
