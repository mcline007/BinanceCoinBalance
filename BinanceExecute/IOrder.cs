using System;

namespace BinanceExecute
{
    public interface IOrder
    {
        int OrderId { get; }
        String Symbol { get; }
        double Amount { get; }
        double Price { get; }
        IBinanceDataPool BinanceDataPool { get; }
        void PlaceBuyOrder();
        void PlaceSellOrder();
        Order.OrderStatuses CheckOrder();
        bool CancelOrder();
    }
}