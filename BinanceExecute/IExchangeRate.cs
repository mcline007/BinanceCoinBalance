using System;
using System.Collections.Generic;

namespace BinanceExecute
{
    public interface IExchangeRate
    {
        String ExchangeRateSymbol { get; }
        double Price { get; }
        double InitialPrice { get; }
        Dictionary<DateTime, double> ExhangeHistory { get; }
        double CurrencyPerformancePercentage { get; }
        double CurrencyPerformance { get; }
        bool IsActive { get; }
        ICurrency MainCurrency { get; }
        ICurrency ReferenceCurrency { get; }
        IBinanceDataPool BinanceDataPool { get; }
        ITrend Trend { get; }
        IExchangeRate referenceUsdExchangeRate { get; }
        TimeSpan MaxHistory { get; }
        double GetPerformance(DateTime dateTime);
        double GetPerformance(TimeSpan timeSpan);
        double GetPerformancePercentage(DateTime dateTime);
        double GetPerformancePercentage(TimeSpan timeSpan);
        double GetPriceInUsd(DateTime dateTime, ICurrency currency);
        string ToString();
    }
}