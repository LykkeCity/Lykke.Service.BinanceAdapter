using Lykke.HttpClientGenerator;

namespace Lykke.Service.BinanceAdapter.Client
{
    public class BinanceAdapterClient : IBinanceAdapterClient
    {
        //public IControllerApi Controller { get; }
        
        public BinanceAdapterClient(IHttpClientGenerator httpClientGenerator)
        {
            //Controller = httpClientGenerator.Generate<IControllerApi>();
        }
        
    }
}
