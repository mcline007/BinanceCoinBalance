using System;
using System.Collections.Generic;
using BinanceClient;

namespace BinanceExecute
{
    public interface IBinanceDataPool
    {
        bool IsActive { get; }
        TimeSpan PriceRefreshPeriod { get; }
        IBinanceService BinanceService { get; }
        Action<List<Prices>> OnExchangeRateReady { set; get; }
        void Start(TimeSpan priceRefreshPeriod);
        void AddTradeInfo(KeyValuePair<String, Action<List<TradeInfo>>> onTradeInfoDataReady);
        void removeTradeInfo(KeyValuePair<String, Action<List<TradeInfo>>> onTradeInfoDataReady);
        List<TradeInfo> GetTradeHistory(String symbol);
        AccountInfo GetAccountInfo();
        OrderInfo PlaceBuyOrder(String symbol, double amount, double price);
        OrderInfo PlaceSellOrder(String symbol, double amount, double price);
        OrderInfo CheckOrder(String symbol, int orderId);
        bool CancelOrder(String symbol, int orderId);
        void Stop();
    }
}