using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using System.Diagnostics;
using BinanceClient;

using System.Threading.Tasks;

namespace BinanceExecute
{
    public class BinanceDataPool : IBinanceDataPool
    {
        private readonly Timer _priceCheckTimer;
        private readonly Object _lock = new Object();
        private IDictionary<String, Action<List<TradeInfo>>> _tradeDataCallbacks;
        
        public bool IsActive { private set; get; }
        public TimeSpan PriceRefreshPeriod { private set; get; }
        public IBinanceService BinanceService { private set; get; }
        public Action<List<Prices>> OnExchangeRateReady { set; get; }

        public BinanceDataPool( IBinanceService binanceService)
        {
            BinanceService = binanceService;
            _tradeDataCallbacks = new Dictionary<String, Action<List<TradeInfo>>>();
            _priceCheckTimer = new Timer(state => Refresh(), binanceService, Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
        }

        public void Start(TimeSpan priceRefreshPeriod)
        {
            PriceRefreshPeriod = priceRefreshPeriod;
            _priceCheckTimer.Change(TimeSpan.Zero, PriceRefreshPeriod);
            IsActive = true;
        }
        public void AddTradeInfo(KeyValuePair<String, Action<List<TradeInfo>>> onTradeInfoDataReady)
        {
            lock (_tradeDataCallbacks)
            {
                if (onTradeInfoDataReady.Key.Contains(onTradeInfoDataReady.Key))
                {
                    _tradeDataCallbacks[onTradeInfoDataReady.Key] += onTradeInfoDataReady.Value;
                }
                else
                {
                    _tradeDataCallbacks.Add(onTradeInfoDataReady);
                }
            }
        }
        public void removeTradeInfo(KeyValuePair<String, Action<List<TradeInfo>>> onTradeInfoDataReady)
        {
            lock (_tradeDataCallbacks)
            {
                if (onTradeInfoDataReady.Key.Contains(onTradeInfoDataReady.Key))
                {
                    _tradeDataCallbacks[onTradeInfoDataReady.Key] -= onTradeInfoDataReady.Value;
                }
            }
        }

        private void Refresh()
        {
            if (Monitor.TryEnter(_lock))
            {
                try
                {
                    List<Prices> latestPrices = BinanceService.ListPrices();
                    OnExchangeRateReady?.Invoke(latestPrices);

                    lock (_tradeDataCallbacks)
                    {
                        foreach (KeyValuePair<String, Action<List<TradeInfo>>> entry in _tradeDataCallbacks)
                        {
                            if (entry.Value != null)
                            {
                                List<TradeInfo> tradeInfo = BinanceService.GetTrades(entry.Key);
                                entry.Value.Invoke(tradeInfo);
                            }
                        }
                    }
                }
                finally
                {
                    Monitor.Exit(_lock);
                }
            }
        }
        public List<TradeInfo> GetTradeHistory(String symbol)
        {
            lock (_lock)
            {
                return BinanceService.GetTrades(symbol);
            }
        }
        public AccountInfo GetAccountInfo()
        {
            lock (_lock)
            {
                return BinanceService.GetAccountInfo(); ;
            }
        }
        public OrderInfo PlaceBuyOrder(String symbol, double amount, double price)
        {
            lock (_lock)
            {
                return BinanceService.PlaceBuyOrder(symbol, amount, price, "LIMIT");
            }
        }

        public OrderInfo PlaceSellOrder(String symbol, double amount, double price)
        {
            lock (_lock)
            {
                return BinanceService.PlaceSellOrder(symbol, amount, price, "LIMIT");
            }
        }

        public OrderInfo CheckOrder(String symbol, int orderId)
        {
            lock (_lock)
            {
                return BinanceService.CheckOrderStatus(symbol, orderId);
            }

        }
        public bool CancelOrder(String symbol, int orderId)
        {
            lock (_lock)
            {
                return BinanceService.CancelOrder(symbol, orderId);
            }

        }
        public void Stop()
        {
            _priceCheckTimer.Change(TimeSpan.MaxValue, TimeSpan.MaxValue);
            IsActive = false;
        }
    }
}
