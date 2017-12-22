using System;
using System.Collections.Generic;

namespace BinanceExecute
{
    public interface IAccount
    {
        double AccountTotalInUsd { get; }
        Dictionary<DateTime, Dictionary<ICurrency, double>> BalanceHistory { get; }
        IDictionary<ICurrency, double> Balances { get; }
        IDictionary<ICurrency, double> BalancesInUsd { get; }
        IBinanceDataPool BinanceDataPool { get; }
        double ChangeAccountTotalInUsd { get; }
        double InitialAccountTotalInUsd { get; }
        IDictionary<ICurrency, double> InitialBalancesInUsd { get; }
        TimeSpan MaxHistory { get; }
        List<IExchangeRate> ExchangeRates { get; }
        List<ICurrency> SupportedCurrencies { get; }

        void RefreshHoldings();
        string ToString();
    }
}