using Autofac;
using Lykke.Service.BinanceAdapter.Services;
using Lykke.Service.BinanceAdapter.Settings;
using Lykke.SettingsReader;
using Microsoft.Extensions.Hosting;

namespace Lykke.Service.BinanceAdapter.Modules
{    
    public class ServiceModule : Module
    {
        private readonly IReloadingManager<AppSettings> _appSettings;

        public ServiceModule(IReloadingManager<AppSettings> appSettings)
        {
            _appSettings = appSettings;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(_appSettings.CurrentValue.BinanceAdapterService.OrderBooks)
                .AsSelf()
                .SingleInstance();

            builder.RegisterType<OrderBookPublishingService>()
                .AsSelf()
                .As<IHostedService>()
                .SingleInstance();
        }
    }
}
