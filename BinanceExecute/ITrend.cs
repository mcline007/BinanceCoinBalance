using System;

namespace BinanceExecute
{
    public interface ITrend
    {
        Trend.TrendStatus GetTrendStatus(IExchangeRate exchangeRate, TimeSpan trendSpan, double initialValue, double currentValue, double limitCommission, double lossTolerance);
        double GetChangePercentage(IExchangeRate exchangeRate, TimeSpan span);
        double GetChangePercentage(IExchangeRate exchangeRate, DateTime endDate, TimeSpan timeSpan);
        Trend.TrendStatus CalculateTrend(IExchangeRate exchangeRate, int divisor = 3);
    }
}