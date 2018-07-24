using Lykke.Common.ExchangeAdapter.Server;
using Lykke.Service.BinanceAdapter.Services;

namespace Lykke.Service.BinanceAdapter.Controllers
{
    public sealed class OrderBooksController : OrderBookControllerBase
    {
        public OrderBooksController(OrderBookPublishingService service)
        {
            Session = service.Session;
        }

        protected override OrderBooksSession Session { get; }
    }
}
