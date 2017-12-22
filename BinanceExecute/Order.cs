using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using BinanceClient;

namespace BinanceExecute
{
    public class Order
    {
        private OrderStatuses _orderStatus;

        public enum OrderStatuses
        {
            New,
            Pending,
            Cancelled,
            Completed,
        }
        public int OrderId { private set; get; }
        public String Symbol { private set; get; }
        public double Amount { private set; get; }
        public IBinanceDataPool BinanceDataPool { private set; get; }

        public Order(String symbol, double amount, IBinanceDataPool binanceDataPool)
        {
            Amount = amount;
            Symbol = symbol;
            BinanceDataPool = binanceDataPool;
            _orderStatus = OrderStatuses.New;
        }

        public void PlaceBuyOrder(double price)
        {
            OrderInfo info = BinanceDataPool.PlaceBuyOrder(Symbol, Amount, price);
            OrderId = int.Parse(info.orderId);
            _orderStatus = OrderStatuses.Pending;
        }

        public void PlaceSellOrder(double price)
        {
            OrderInfo info = BinanceDataPool.PlaceSellOrder(Symbol, Amount, price);
            OrderId = int.Parse(info.orderId);
            _orderStatus = OrderStatuses.Pending;
        }
        
        async public Task<bool> WaitForOrder(TimeSpan timeOut, OrderStatuses status)
        {
            if (_orderStatus == status)
            {
                return true;
            }

            var result = await Task.Factory.StartNew(async () =>
            {
                DateTime startTime = DateTime.Now;
                while ((DateTime.Now - startTime) > timeOut)
                {
                    _orderStatus = CheckOrder();

                    if (_orderStatus == status)
                    {
                        return true;
                    }
                }
                return false;
            });
            return result.Result;
        }

        public OrderStatuses CheckOrder()
        {
            if (_orderStatus == OrderStatuses.Pending)
            {
                OrderInfo info = BinanceDataPool.CheckOrder(Symbol, OrderId);
                _orderStatus = info.status == "CANCELED" ? OrderStatuses.Cancelled :
                    (info.status == "FILLED" ? OrderStatuses.Completed : OrderStatuses.Pending);
            }
            return _orderStatus;
        }

        public bool CancelOrder()
        {
            if (BinanceDataPool.CancelOrder(Symbol, OrderId))
            {
                _orderStatus = OrderStatuses.Cancelled;
                return true;
            }
            _orderStatus = OrderStatuses.Completed;
            return false;
        }
    }
}
