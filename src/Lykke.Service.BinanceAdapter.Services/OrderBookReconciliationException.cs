using System;

namespace Lykke.Service.BinanceAdapter.Services
{
    public class OrderBookReconciliationException : Exception
    {
        public OrderBookReconciliationException(long currentUpdateId, long updateFirstId, long updateLastId)
            : base($"Current updateId: {currentUpdateId}, got update with {updateFirstId}..{updateLastId}")
        {
        }
    }
}