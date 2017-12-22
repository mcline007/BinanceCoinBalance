using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinanceClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BinanceExecute
{
    public class Account : IAccount
    {
        private Dictionary<DateTime, Dictionary<ICurrency, double>> _balanceHistory;

        public TimeSpan MaxHistory { private set; get; }


        public IDictionary<ICurrency, double> Balances
        {
            get
            {
                return BalanceHistory.Last().Value;
            }
        }
        public Dictionary<DateTime, Dictionary<ICurrency, double>> BalanceHistory
        {
            get
            {
                return _balanceHistory;
            }
        }
        public double AccountTotalInUsd
        {
            get
            {
                return BalancesInUsd.Values.Sum();
            }
        }
        public double InitialAccountTotalInUsd
        {
            get
            {
                return InitialBalancesInUsd.Values.Sum();
            }
        }
        public double ChangeAccountTotalInUsd
        {
            get
            {
                return AccountTotalInUsd - InitialAccountTotalInUsd;
            }
        }

        public IDictionary<ICurrency, double> BalancesInUsd
        {
            get
            {
                IDictionary<ICurrency, double> balancesUsd = Balances.ToDictionary(currency => currency.Key,
                balance =>
                {
                    IExchangeRate coinUsdExchangeRate = ExchangeRates.FirstOrDefault(ex => ex.ExchangeRateSymbol == balance.Key.Symbol + Currency.UsDollar.Symbol);
                    return balance.Value * coinUsdExchangeRate.Price;
                });

                List<KeyValuePair<ICurrency, double>> balancesUsdList = balancesUsd.ToList();
                balancesUsdList.Sort(
                    delegate (KeyValuePair<ICurrency, double> pair1,
                    KeyValuePair<ICurrency, double> pair2)
                    {
                        return pair1.Value.CompareTo(pair2.Value);
                    }
                );

                balancesUsd.Clear();
                balancesUsdList.Reverse();
                balancesUsdList.ForEach(balance => balancesUsd.Add(balance));

                return balancesUsd;
            }
        }

        public IDictionary<ICurrency, double> InitialBalancesInUsd
        {
            get
            {
                return Balances.ToDictionary(currency => currency.Key,
                balance =>
                {
                    IExchangeRate coinUsdExchangeRate = ExchangeRates.FirstOrDefault(ex => ex.ExchangeRateSymbol == balance.Key.Symbol + Currency.UsDollar.Symbol);
                    if (coinUsdExchangeRate == null)
                    {
                        IExchangeRate bitcointExchangeRate = ExchangeRates.FirstOrDefault(ex =>
                        ex.ExchangeRateSymbol == balance.Key.Symbol + Currency.Bitcoin.Symbol);

                        IExchangeRate usdToBitcoinExhangeRate = ExchangeRates.FirstOrDefault(ex =>
                            ex.ExchangeRateSymbol == Currency.Bitcoin.Symbol + Currency.UsDollar.Symbol);

                        return (balance.Value / bitcointExchangeRate.InitialPrice) * usdToBitcoinExhangeRate.InitialPrice;
                    }
                    else
                    {
                        return balance.Value * coinUsdExchangeRate.InitialPrice;
                    }
                });
            }
        }

        public List<ICurrency> SupportedCurrencies { private set; get; }
        public List<IExchangeRate> ExchangeRates { private set; get; }
        public IBinanceDataPool BinanceDataPool { private set; get; }

        public Account(IBinanceDataPool binanceDataPool, List<ICurrency> currencies, List<IExchangeRate> exchangeRates, TimeSpan maxHistory)
        {
            _balanceHistory = new Dictionary<DateTime, Dictionary<ICurrency, double>>();
            SupportedCurrencies = currencies;
            MaxHistory = maxHistory;
            BinanceDataPool = binanceDataPool;
            ExchangeRates = exchangeRates;
        }

        public override string ToString()
        {
            return Environment.NewLine + String.Join("\n",BalancesInUsd.Keys.Select(
                    balance => balance.Symbol + " : $" + (BalancesInUsd[balance]).ToString("N2") + ", Initial Value : $" +
                    (InitialBalancesInUsd[balance]).ToString("N2") + ", Change : $" +
                    ((BalancesInUsd[balance] - InitialBalancesInUsd[balance])).ToString("N2")  )) + 
                    "\nAccount Total : $" + AccountTotalInUsd.ToString("N2") + ", Initial Account Total: $" + InitialAccountTotalInUsd.ToString("N2")
                    + ", Change Account Total: $" + ChangeAccountTotalInUsd.ToString("N2");
        }

        /*public double GetEarningsInUsd(ICurrency currency)
        {
            Dictionary<DateTime, Dictionary<ICurrency, double>> balanceHistory = BalanceHistory;


            double balance = 0.0;
            double previousBalance = 0.0;
            foreach (KeyValuePair<DateTime, Dictionary<ICurrency, double>> balanceHistoricalEntry in balanceHistory)
            {
                if (!balanceHistoricalEntry.Value.ContainsKey(currency))
                {
                  continue;
                }

                IDictionary<ICurrency, double> entry = balanceHistoricalEntry.Value;
                if (Math.Abs(previousBalance - entry[currency]) > double.Epsilon)
                {
                    IExchangeRate exchangeRate = _exchangeRates.First(exrat => exrat.ReferenceCurrency == currency);
                    double price = exchangeRate.GetPriceInUsd(balanceHistoricalEntry.Key, currency);


                }
                previousBalance = balanceEntry[currency];
            }
        }*/
        public void RefreshHoldings()
        {
            AccountInfo accountInfo = BinanceDataPool.GetAccountInfo();

            Dictionary<ICurrency, double> balances = new Dictionary<ICurrency, double>();
            foreach (AccountInfo.Balance balance in accountInfo.balances)
            {
                double balanceD = double.Parse(balance.free);

                ICurrency currency = SupportedCurrencies.FirstOrDefault(curr => curr.Symbol == balance.asset);
                if (currency != null)
                {
                    balances.Add(currency, balanceD);
                }
            }

            Dictionary<DateTime, Dictionary<ICurrency, double>> balanceHistory = _balanceHistory.Where(pair => pair.Key > DateTime.Now - MaxHistory)
                .ToDictionary(pair => pair.Key, pair => pair.Value);

            balanceHistory.Add(DateTime.Now, balances);
            _balanceHistory = balanceHistory;
        }
    }
}
