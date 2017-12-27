using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Timers;

using BinanceClient;
using Timer = System.Threading.Timer;


namespace BinanceExecute
{
    public class ExchangeRate : IExchangeRate
    {
        private const double _trendPriority = 0.01;

        private Dictionary<DateTime, double> _exchangeRateHistory = new Dictionary<DateTime, double>();

        public String ExchangeRateSymbol => MainCurrency.Symbol + ReferenceCurrency.Symbol;

        public double Price
        {
            get
            {
                return _exchangeRateHistory.Last().Value;
            }
        }
        public double InitialPrice
        {
            get
            {
                return _exchangeRateHistory.First().Value;
            }
        }
        public Dictionary<DateTime, double> ExhangeHistory
        {
            get
            {
                return _exchangeRateHistory;
            }
        }
        public double CurrencyPerformancePercentage
        {
            get
            {
                return GetPerformancePercentage(Process.GetCurrentProcess().StartTime);
            }
        }
        public double CurrencyPerformance
        {
            get
            {
                return GetPerformance(Process.GetCurrentProcess().StartTime);
            }
        }
        public bool IsActive { private set;  get; }
        public ICurrency MainCurrency { private set;  get; }
        public ICurrency ReferenceCurrency { private set;  get; }
        public IBinanceDataPool BinanceDataPool { private set; get; }
        public ITrend Trend { private set; get; }

        public IExchangeRate referenceUsdExchangeRate { private set; get; }
        public TimeSpan MaxHistory { private set; get; }

        public ExchangeRate(ICurrency mainCurrency, ICurrency referenceCurrency,
            IBinanceDataPool binanceDataPool, TimeSpan maxHistory)
        {
            Trend = new Trend();
            MaxHistory = maxHistory;
            MainCurrency = mainCurrency;
            ReferenceCurrency = referenceCurrency;

            if (mainCurrency != Currency.Bitcoin)
            {
                referenceUsdExchangeRate = new ExchangeRate(Currency.Bitcoin, Currency.UsDollar,
                    binanceDataPool, maxHistory);
            }

            BinanceDataPool = binanceDataPool;
            BinanceDataPool.OnExchangeRateReady += OnPriceChange;
        }

        private void OnPriceChange(List<Prices> prices)
        {
            double priceOfSymbol = double.NaN;
            if (prices.Any(price => price.Symbol == ExchangeRateSymbol))
            {
                priceOfSymbol = (from p in prices
                    where p.Symbol == ExchangeRateSymbol
                    select p.Price).First();
            }
            else
            {
                double priceOfSymbolInbtc = (from p in prices
                                 where p.Symbol == MainCurrency.Symbol + referenceUsdExchangeRate.MainCurrency.Symbol
                                 select p.Price).First();

                priceOfSymbol = priceOfSymbolInbtc * referenceUsdExchangeRate.Price;
            }

            Dictionary<DateTime, double> exchangeRateHistory = _exchangeRateHistory.Where(pair => pair.Key > DateTime.Now - MaxHistory)
                .ToDictionary(pair => pair.Key, pair => pair.Value);

            exchangeRateHistory.Add(DateTime.Now, priceOfSymbol);
            _exchangeRateHistory = exchangeRateHistory;
        }

        public double GetPerformance(DateTime dateTime)
        {
            return GetPerformance(DateTime.Now - dateTime);
        }
        public double GetPerformance(TimeSpan timeSpan)
        {
            Dictionary<DateTime, double> matches = _exchangeRateHistory.Where(per => per.Key > DateTime.Now - timeSpan).
                ToDictionary(i => i.Key, i => i.Value);

            if (!matches.Any())
            {
                return 0.0;
            }
            return matches.Values.First() - matches.Values.Last();

        }
        public double GetPerformancePercentage(DateTime dateTime)
        {
            return GetPerformancePercentage(DateTime.Now - dateTime);
        }
        public double GetPerformancePercentage(TimeSpan timeSpan)
        {
            Dictionary<DateTime, double> matches = _exchangeRateHistory.Where(per => per.Key > DateTime.Now - timeSpan).
                ToDictionary(i => i.Key, i => i.Value);

            if (!matches.Any())
            {
                return 0.0;
            }
            return ((matches.Values.First() - matches.Values.Last()) / matches.Values.First()) * 100;

        }

        public double GetPriceInUsd(DateTime dateTime, ICurrency currency)
        {
            Dictionary<DateTime, double> exhangeHistory = ExhangeHistory;
            return exhangeHistory.First(pair => pair.Key > dateTime).Value;
        }
        public override string ToString()
        {
            return "\n" + ExchangeRateSymbol + " -> Price $" + Price.ToString("N3");
        }

        ~ExchangeRate()
        {
            BinanceDataPool.OnExchangeRateReady -= OnPriceChange;
        }
    }
}
