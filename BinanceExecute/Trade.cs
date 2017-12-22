using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using BinanceClient;

namespace BinanceExecute
{
    public class Trade
    {
        private readonly List<TradeInfo> _trades = new List<TradeInfo>();

        public IBinanceDataPool BinanceDataPool { private set; get; }
        public ICurrency MainCurrency { private set; get; }
        public ICurrency ReferenceCurrency { private set; get; }
        public ICurrency EndCurrency { private set; get; }

        public List<IExchangeRate> UsExchangeRates { private set; get; }

        public Action<List<TradeInfo>> OnTradeUpdate { set; get; }
        public AutoResetEvent NewTradeTransactionEvent { private set; get; }

        public List<TradeInfo> TradeHistory
        {
            get
            {
                lock(_trades)
                {
                    TradeInfo[] trades = new TradeInfo[_trades.Count];
                    _trades.CopyTo(trades, 0);
                    return trades.ToList();
                }
            }
        }

        public Trade(IBinanceDataPool binanceDataPool, ICurrency oldCurrency, ICurrency newCurrency, List<IExchangeRate> exchangeRates)
        {
            BinanceDataPool = binanceDataPool;
            BinanceDataPool.AddTradeInfo(new KeyValuePair<string, Action<List<TradeInfo>>>(MainCurrency.Symbol + 
                ReferenceCurrency.Symbol, OnNewTradeInfoEntry));

            UsExchangeRates = exchangeRates.Where(exchangeRate =>
                exchangeRate.ReferenceCurrency == newCurrency).ToList();
        }

        public void OnNewTradeInfoEntry(List<TradeInfo> trades)
        {
            List<TradeInfo> newEntries = new List<TradeInfo>();
            lock (_trades)
            {
                _trades.Clear();
                _trades.AddRange(trades);
            }

            if(newEntries.Any())
            {
                NewTradeTransactionEvent.Set();
            }
            OnTradeUpdate?.Invoke(newEntries);
        }
    }
}
