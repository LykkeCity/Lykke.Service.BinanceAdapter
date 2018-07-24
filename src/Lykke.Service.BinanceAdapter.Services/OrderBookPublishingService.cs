using System;
using System.Linq;
using System.Net;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Common.ExchangeAdapter;
using Lykke.Common.ExchangeAdapter.Contracts;
using Lykke.Common.ExchangeAdapter.Server;
using Lykke.Common.ExchangeAdapter.Server.Settings;
using Lykke.Common.ExchangeAdapter.Tools.ObservableWebSocket;
using Lykke.Common.Log;
using Lykke.Service.BinanceAdapter.Services.RestClient.Contracts;
using Lykke.Service.BinanceAdapter.Services.WebSocket;
using Lykke.SettingsReader.Attributes;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Lykke.Service.BinanceAdapter.Services
{
    public sealed class OrderBookPublishingService : IHostedService
    {
        private readonly OrderBookProcessingSettings _settings;
        private readonly ILogFactory _lf;
        private CompositeDisposable _disposable;
        private readonly RestClient.RestClient _client;
        private readonly ILog _log;

        public OrderBookPublishingService(OrderBookProcessingSettings settings, ILogFactory lf)
        {
            _settings = settings;
            _lf = lf;
            _client = new RestClient.RestClient(lf, settings.OrderBookDepth);
            _log = lf.CreateLog(this);
        }

        public OrderBooksSession Session { get; private set; }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Session = await CreateSession();

            _disposable = new CompositeDisposable(
                Session,
                Session.Worker.Subscribe());
        }

        private async Task<OrderBooksSession> CreateSession()
        {
            var exchangeInfo = await _client.GetExchangeInfo();

            var assets = exchangeInfo.Assets.ToArray();

            var streams = string.Join("/", assets.Select(x => $"{WebUtility.UrlEncode(x)?.ToLower()}@depth"));

            var orderBooks = ReadDepthUpdates(
                    new ObservableWebSocket(
                        $"wss://stream.binance.com:9443/stream?streams={streams}",
                        x => _log.Info(x)))
                .GroupBy(x => x.Asset)
                .SelectMany(CombineWithSnapshot);

            return orderBooks.FromRawOrderBooks(assets, _settings, _lf);
        }

        private IObservable<OrderBook> CombineWithSnapshot(IGroupedObservable<string, DepthUpdate> grouped)
        {
            // https://github.com/binance-exchange/binance-official-api-docs/blob/master/web-socket-streams.md#how-to-manage-a-local-order-book-correctly
            //
            // How to manage a local order book correctly
            //
            // Open a stream to wss://stream.binance.com:9443/ws/bnbbtc@depth
            // Buffer the events you receive from the stream
            // Get a depth snapshot from https://www.binance.com/api/v1/depth?symbol=BNBBTC&limit=1000
            // Drop any event where u is <= lastUpdateId in the snapshot
            // The first processed should have U <= lastUpdateId+1 AND u >= lastUpdateId+1
            // While listening to the stream, each new event's U should be equal to the previous event's u+1
            // The data in each event is the absolute quantity for a price level
            //     If the quantity is 0, remove the price level
            // Receiving an event that removes a price level that is not in your local order book can happen and is normal.

            return Observable.FromAsync(async () =>
                {
                    var binanceOb = await _client.GetOrderBook(grouped.Key);
                    return (binanceOb.LastUpdateId, binanceOb.ToOrderBook(grouped.Key));
                })
                .SelectMany(restOb =>
                {
                    var (seedUpdateId, seedOb) = restOb;

                    return grouped
                        .SkipWhile(x => x.LastUpdateId <= seedUpdateId)
                        .Scan((seedUpdateId, seedOb, false), ApplyOrderBookUpdate)
                        .Select(x => x.Item2);
                });
        }

        private static (long, OrderBook, bool) ApplyOrderBookUpdate((long, OrderBook, bool) current, DepthUpdate update)
        {
            var (lastUpdateId, ob, strictCheckVersion) = current;

            if (!strictCheckVersion)
            {
                if (update.FirstUpdateId > lastUpdateId + 1 ||
                    update.LastUpdateId < lastUpdateId + 1)
                {
                    throw new OrderBookReconciliationException(
                        lastUpdateId,
                        update.FirstUpdateId,
                        update.LastUpdateId);
                }
            }
            else
            {
                if (update.FirstUpdateId != lastUpdateId + 1)
                {
                    throw new OrderBookReconciliationException(
                        lastUpdateId,
                        update.FirstUpdateId,
                        update.LastUpdateId);
                }
            }

            var newOb = ob.Clone(update.EventTimeMs.FromEpochMilliseconds());

            foreach (var ask in update.Asks)
            {
                newOb.UpdateAsk(ask[0].Value<decimal>(), ask[1].Value<decimal>());
            }

            foreach (var bid in update.Bids)
            {
                newOb.UpdateBid(bid[0].Value<decimal>(), bid[1].Value<decimal>());
            }

            return (update.LastUpdateId, newOb, true);
        }

        private IObservable<DepthUpdate> ReadDepthUpdates(IObservable<ISocketEvent> source)
        {
            return source.Select(x =>
                {
                    if (x is IMessageReceived<byte[]> msg)
                    {
                        return JsonConvert.DeserializeObject<StreamMessage<DepthUpdate>>(
                            Encoding.UTF8.GetString(msg.Content)).Data;
                    }

                    return null;
                })
                .Where(x => x != null);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _disposable?.Dispose();

            return Task.CompletedTask;
        }
    }
}
