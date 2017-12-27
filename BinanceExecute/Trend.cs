using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using alglib2;


namespace BinanceExecute
{
    public class Trend : ITrend
    {
        private const int MinimumSize = 10;

        public enum TrendStatus
        {
            BuyNow,
            Buy,
            Keep ,
            Sell,
            SellNow,
            NotEnoughData,
            Inconlusive,
        }

     /*   public void CalculateDerivatives(IDictionary<double, double> inputPoints, out IDictionary<double, double> firstDerivatives,
            out IDictionary<double, double> secondDerivatives)
        {

            var inputPointsXArray = inputPoints.Keys.ToArray();
            var inputPointsYArray = inputPoints.Values.ToArray();

            spline1dinterpolant akimaSplineToDifferentiate;
            alglib.spline1dbuildakima(inputPointsXArray, inputPointsYArray, out akimaSplineToDifferentiate);

            firstDerivatives = new Dictionary<double, double>();
            secondDerivatives = new Dictionary<double, double>();
            foreach (var pair in inputPoints)
            {
                var xPoint = pair.Key;
                double functionVal, firstDeriv, secondDeriv;
                alglib.spline1ddiff(akimaSplineToDifferentiate, xPoint, out functionVal, out firstDeriv, out secondDeriv);

                firstDerivatives.Add(xPoint, firstDeriv);
                secondDerivatives.Add(xPoint, secondDeriv);
            }
        }*/

        public TrendStatus GetTrendStatus(IExchangeRate exchangeRate, TimeSpan trendSpan, double initialValue, double currentValue, double limitCommission, double lossTolerance)
        {
            const int divider = 3;
            IDictionary<DateTime, double> stockDataPoints = exchangeRate.ExhangeHistory;

            DateTime now = DateTime.Now;
            List<KeyValuePair<DateTime, double>> stockDataPercentagePointsSeconds = new List<KeyValuePair<DateTime, double>>();
           
            List<double> rankings = new List<double>();
            for (int sample = 1; sample < 10; sample*=2)
            {
                rankings.Add(GetChangePercentage(exchangeRate, now - (trendSpan *= sample), trendSpan));
            }
            rankings.RemoveAll(ranking => double.IsNaN(ranking));

            if (rankings.Count < 1)
            {
                return TrendStatus.NotEnoughData; //Not enough data
            }

            double gainLossPercentage = (currentValue - initialValue) / currentValue;
            double expectedGain = gainLossPercentage - limitCommission;

            int sets = (int)Math.Floor(rankings.Count /(double)divider);
            if (Math.Abs(expectedGain) < limitCommission)
            {
                return TrendStatus.Keep; // A stable asset, we should sell if we have better options
            }
            else if (expectedGain > limitCommission)
            {
                if (rankings.Count < divider)
                {
                    return TrendStatus.NotEnoughData; //Not enough data
                }

                if (rankings.Count < divider)
                {
                    return TrendStatus.NotEnoughData; //Not enough data
                }
                if (rankings.All(ranking => ranking > 0))
                {
                    return TrendStatus.Buy; // A asset on the rise, keep it
                }

                if (Math.Abs(rankings.Last()) < limitCommission)
                {
                    return TrendStatus.Keep;
                }

                if (rankings.Last() < -limitCommission)
                {
                    return TrendStatus.Sell;
                }
                if (rankings.Last() > limitCommission)
                {
                    return TrendStatus.Keep; 
                }
            }
            else
            {
                if (expectedGain < -lossTolerance || rankings.First() < -lossTolerance)
                {
                    return TrendStatus.SellNow;
                }

                if (rankings.Count < divider)
                {
                    return TrendStatus.NotEnoughData; //Not enough data
                }

                if (rankings.All(ranking => ranking > 0))
                {
                    return TrendStatus.Buy; // A asset on the rise, keep it
                }

                if (Math.Abs(rankings.Last()) < limitCommission)
                {
                    return TrendStatus.Buy;
                }

                return TrendStatus.Keep; // We dont sell on a loss
            }

            return TrendStatus.Inconlusive;



          /*  IDictionary<double, double> firstDerivatives;
            IDictionary<double, double> secondDerivatives;

            CalculateDerivatives(stockDataPercentagePointsSeconds, out firstDerivatives, out secondDerivatives);*/

        }


        public double GetChangePercentage(IExchangeRate exchangeRate, TimeSpan span)
        {
            return GetChangePercentage(exchangeRate, DateTime.Now, span);
        }
        public double GetChangePercentage(IExchangeRate exchangeRate, DateTime endDate, TimeSpan timeSpan)
        {
            IDictionary<DateTime, double> stockDataPoints = exchangeRate.ExhangeHistory;
            return double.NaN;
            if (stockDataPoints.Count == 0 || endDate - timeSpan <= stockDataPoints.First().Key)
            {
                return double.NaN;
            }

            double end = stockDataPoints.First(pair => pair.Key >= endDate - timeSpan).Value;
            double start = stockDataPoints.First(pair => pair.Key >= endDate).Value;
            if (start == end)
            {
                return 0.0;
            }
            return  (start - end) / start;
        }
        public double GetChangePercentage(IExchangeRate exchangeRate)
        {
            IDictionary<DateTime, double> stockDataPoints = exchangeRate.ExhangeHistory;

            if (stockDataPoints.Count == 0)
            {
                return double.NaN;
            }

            double end = stockDataPoints.First().Value;
            double start = stockDataPoints.Last().Value;
            if (start == end)
            {
                return 0.0;
            }
            return (start - end) / start;
        }
        /*double tertySecondsChange = trend.ChangePercentageToLatest(ownedExchangeRate, TimeSpan.FromMinutes(0.5));
        double oneMinuteChange = trend.ChangePercentageToLatest(ownedExchangeRate, TimeSpan.FromMinutes(1));
        double fiveMinuteChange = trend.ChangePercentageToLatest(ownedExchangeRate, TimeSpan.FromMinutes(5));
        double thertyMinuteChange = trend.ChangePercentageToLatest(ownedExchangeRate, TimeSpan.FromMinutes(30));
        double sixtyMinuteChange = trend.ChangePercentageToLatest(ownedExchangeRate, TimeSpan.FromMinutes(60));
        double twientyFourHoursChange = trend.ChangePercentageToLatest(ownedExchangeRate, TimeSpan.FromHours(23));
        double currencyBalance = (account.BalancesInUsd[ownedExchangeRate.MainCurrency] - account.InitialBalancesInUsd[ownedExchangeRate.MainCurrency]);
        double percentageGrowth = account.InitialBalancesInUsd[ownedExchangeRate.MainCurrency] != 0.0 && account.BalancesInUsd[ownedExchangeRate.MainCurrency] - account.InitialBalancesInUsd[ownedExchangeRate.MainCurrency] != 0.0
            ? 100.0 * (account.BalancesInUsd[ownedExchangeRate.MainCurrency] - account.InitialBalancesInUsd[ownedExchangeRate.MainCurrency]) / account.InitialBalancesInUsd[ownedExchangeRate.MainCurrency]
            : 0.0;
    }

    DateTime now = DateTime.Now;
    DateTime dateTimeInitial = stockDataPoints.First(k => k.Key > now - timeSpan).Key;
    if(stockDataPoints[dateTimeInitial] == stockDataPoints.Last().Value ||
        stockDataPoints.First().Key > now - timeSpan)
    {
        return double.NaN;
    }
    }
}

public double ChangePercentageToLatest(IExchangeRate exchangeRate, TimeSpan timeSpan)
{
    IDictionary<DateTime, double> stockDataPoints = exchangeRate.ExhangeHistory;

    DateTime now = DateTime.Now;
    DateTime dateTimeInitial = stockDataPoints.First(k => k.Key > now - timeSpan).Key;
    if (stockDataPoints[dateTimeInitial] == stockDataPoints.Last().Value ||
        stockDataPoints.First().Key > now - timeSpan)
    {
        return double.NaN;
    }
    return 100.0 * ((stockDataPoints.Last().Value - stockDataPoints[dateTimeInitial]) / stockDataPoints[dateTimeInitial]);
}

public double ChangeWeighedPercentageToLatest(IExchangeRate exchangeRate, TimeSpan timeSpan)
{
    IDictionary<DateTime, double> stockDataPoints = exchangeRate.ExhangeHistory;

    DateTime now = DateTime.Now;
    DateTime dateTimeInitial = stockDataPoints.First(k => k.Key > now - timeSpan).Key;
    List<KeyValuePair<DateTime,double>> entries = stockDataPoints.Where(k => k.Key > now - timeSpan).ToList();
    List<double> weigthedChangesPercentage = entries.Average();

    List <double> weigthedChanges = new List<double>();
    for (int i = 1; i < entries.Count; i++)
    {
        weigthedChanges.Add(entries[i].Value - entries[i - 1].Value);
    }
    return weigthedChanges.Average();

    return 100.0 * ((stockDataPoints.Last().Value - stockDataPoints[dateTimeInitial]) / stockDataPoints[dateTimeInitial]);
}


/*  public TrendStatus CalculateTrends(IExchangeRate exchangeRate, int divisor = 3)
  {
      Acceleration acceleration = new Acceleration()
  }*/
        public TrendStatus CalculateTrend(IExchangeRate exchangeRate, int divisor = 3)
        {
            IDictionary<DateTime, double> stockDataPoints = exchangeRate.ExhangeHistory;
            if (stockDataPoints.Count() < MinimumSize && stockDataPoints.Count() < divisor)
            {
                return TrendStatus.Keep;
            }

            IDictionary<DateTime, double> stockDataPointsDivided = exchangeRate.ExhangeHistory;

            DateTime now = DateTime.Now;
            IDictionary<double, double> stockDataPercentagePointsSeconds = new Dictionary<double, double>();

            int count = 0;
            double previousValue = double.NaN;
            foreach (KeyValuePair<DateTime, double> entry in stockDataPoints)
            {
                if (double.IsNaN(previousValue))
                {
                    previousValue = entry.Value;
                    continue;
                }

                if (count % divisor == 0)

                    stockDataPercentagePointsSeconds.Add((now - entry.Key).TotalSeconds,
                        entry.Value == previousValue ? 0 : ((entry.Value - previousValue) / previousValue) * 100);
                previousValue = entry.Value;
            }


            if (stockDataPercentagePointsSeconds.Count() < 2)
            {
                return TrendStatus.Keep;
            }

            IDictionary<double, double> firstDerivatives;
            IDictionary<double, double> secondDerivatives;

            //CalculateDerivatives(stockDataPercentagePointsSeconds, out firstDerivatives, out secondDerivatives);


            /*String toString = String.Join("----", Enumerable.Range(0, stockDataPercentagePointsSeconds.Count).Select(index =>
                         stockDataPercentagePointsSeconds.Values.ElementAt(index).ToString("N5") + ", " + firstDerivatives.Values.ElementAt(index).ToString("N5") + ", " +
                         secondDerivatives.Values.ElementAt(index).ToString("N5"))); */


            /*
            Console.WriteLine("\n");
            Console.WriteLine("," +  + "," + + ",");
            Console.WriteLine("\n");

    */

            return 0;
        }
    }
}

