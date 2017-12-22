using System;
using System.Collections.Generic;
using System.Threading;
using BinanceClient;

namespace BinanceExecute
{
    public interface ITrade
    {
        IBinanceDataPool BinanceDataPool { get; }
        IExchangeRate ExchangeRate { get; }
        AutoResetEvent NewTradeTransactionEvent { get; }
        Action<List<TradeInfo>> OnTradeUpdate { get; set; }
        List<TradeInfo> TradeHistory { get; }

        void OnNewTradeInfoEntry(List<TradeInfo> trades);
    }
}