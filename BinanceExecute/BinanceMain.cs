using BinanceAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;



using BinanceClient;


namespace BinanceExecute
{
    public class BinanceMain
    {
        public static QueuedLock CursorLock = new QueuedLock();

        public struct WriteLineStruct
        {
            public String Text { set; get; }
            public double Value { set; get; }
            public double Threshold { set; get; }
        }



        static void Main(string[] args)
        {
            if(args.Count() != 2)
            {
                Console.WriteLine("Error. Usage BinanceExecute.exe [binance key] [binance secret]");
                Console.WriteLine("Press enter to exit");
                Console.ReadLine();
                Environment.Exit(0);
            }

            TimeSpan maxTradeData = TimeSpan.FromHours(12);
            TimeSpan refreshData = TimeSpan.FromSeconds(1);

            string key = args[0];
            string secret = args[1];

            var binanceClient = new BinanceClient.BinanceClient(key, secret);
            var binanceService = new BinanceService(binanceClient);

            IBinanceDataPool binanceDataPool = new BinanceDataPool(binanceService);
            binanceDataPool.Start(refreshData);

            List<ICurrency> allCurrencies = Currency.AllCurrencies.Where(curr => curr != Currency.UsDollar).ToList();
            List<IExchangeRate> allExchangeRates = allCurrencies.Select(curr => (IExchangeRate)new ExchangeRate(curr, Currency.UsDollar,
                binanceDataPool, maxTradeData)).ToList();

            IExchangeRate liteCoinExchangeRate = allExchangeRates.FirstOrDefault(exc => exc.ReferenceCurrency == Currency.UsDollar && exc.MainCurrency == Currency.LiteCoin);

            Account account = new Account(binanceDataPool, allCurrencies, allExchangeRates, maxTradeData);
            DoWork(account,  refreshData, binanceDataPool, false);

            /* List<ITrade> trades = allCurrencies.Select(currency => allCurrencies.
                     Select(referenceCurrency => new KeyValuePair<ICurrency, ICurrency>(currency, referenceCurrency)).
                     Where(keyPairVal => keyPairVal.Key != keyPairVal.Value)).
                 SelectMany(keyPairVals => keyPairVals).Select(keyPairVal => 
                 new Trade(binanceDataPool, keyPairVal.Key, keyPairVal.Value, allExchangeRates) as ITrade).ToList();

             */


            /*IExchangeRate exchangeRate = new  ExchangeRate(Currency.Ethereum, Currency.Bitcoin, binanceService, exchangeRatePool, maxTradeData);
                        Order order = new Order(exchangeRate, 0.0056, binanceService);
                        order.PlaceBuyOrder();
                        */



            /*
                        var getAccount = binanceService.GetAccountAsync();
                        Task.WaitAll(getAccount);
                        dynamic account = getAccount.Result;
                        Console.WriteLine(account);*/

            //GET TICKER DEPTH
            /*var getDepth = binanceService.GetDepthAsync("BTCUSDT");
            Task.WaitAll(getDepth);
            dynamic depth = getDepth.Result;
            Console.WriteLine(depth);*/

            /*IExchangeRatePool exchangeRatePool = new BinanceDataPool(binanceService);
            exchangeRatePool.Start(TimeSpan.FromSeconds(0.5));

            IExchangeRate bitCoinExchangeRate = new ExchangeRate(Currency.Bitcoin, Currency.UsDollar, binanceService, exchangeRatePool, maxTradeData);
           *IExchangeRate binanceCoinExchangeRate = new ExchangeRate(Currency.BinanceCoin, Currency.UsDollar, binanceService, exchangeRatePool, maxTradeData);
            IExchangeRate ethereumExchangeRate = new ExchangeRate(Currency.Ethereum, Currency.UsDollar, binanceService, exchangeRatePool, maxTradeData);
            IExchangeRate liteCoinExchangeRate = new ExchangeRate(Currency.LiteCoin, Currency.UsDollar, binanceService, exchangeRatePool, maxTradeData);

            
            Thread.Sleep(500000);*/


            /*
            //GET ACCOUNT INFORMATION
            var getAccount = binanceService.GetAccountAsync();
            Task.WaitAll(getAccount);
            dynamic account = getAccount.Result;
            Console.WriteLine(account);

            //GET ORDERS FOR SYMBOL
            var getOrders = binanceService.GetOrdersAsync("BNBBTC", 100);
            Task.WaitAll(getOrders);
            dynamic orders = getOrders.Result;
            Console.WriteLine(orders);

            //GET MY TRADES
            var getMyTrades = binanceService.GetTradesAsync("WTCBTC");
            Task.WaitAll(getAccount);
            dynamic trades = getMyTrades.Result;
            Console.WriteLine(trades);

            //GET ALL PRICES
            List<Prices> prices = new List<Prices>();
            prices = binanceService.ListPrices();
            Console.WriteLine(prices);*/

            //GET PRICE OF SYMBOL
            /*double symbol = binanceService.GetPriceOfSymbol("BTCUSDT");
            Console.WriteLine("Price of BNB: " + symbol);*/

            /*//PLACE BUY ORDER
            var placeBuyOrder = binanceService.PlaceBuyOrderAsync("NEOBTC", 1.00, 00.008851);
            Task.WaitAll(placeBuyOrder);
            dynamic buyOrderResult = placeBuyOrder.Result;
            Console.WriteLine(buyOrderResult);

            //PLACE SELL ORDER
            var placeSellOrder = binanceService.PlaceSellOrderAsync("NEOBTC", 1.00, 00.008851);
            Task.WaitAll(placeSellOrder);
            dynamic sellOrderResult = placeSellOrder.Result;
            Console.WriteLine(sellOrderResult);

            //TEST ORDER---------------------------------------------------------------------------
            var placeOrderTest = binanceService.PlaceTestOrderAsync("NEOBTC", "SELL", 1.00, 00.006151);
            Task.WaitAll(placeOrderTest);
            dynamic testOrderResult = placeOrderTest.Result;
            Console.WriteLine(testOrderResult);
            //-------------------------------------------------------------------------------------

            //CHECK ORDER STATUS BY ID
            var checkOrder = binanceService.CheckOrderStatusAsync("NEOBTC", 5436663);
            Task.WaitAll(checkOrder);
            dynamic checkOrderResult = checkOrder.Result;
            Console.WriteLine(checkOrderResult);

            //DELETE ORDER BY ID
            var deleteOrder = binanceService.CancelOrderAsync("NEOBTC", 5436663);
            Task.WaitAll(deleteOrder);
            dynamic deleteOrderResult = deleteOrder.Result;
            Console.WriteLine(deleteOrderResult);*/

            Console.ReadLine();
        }
/*
        public static void WatchHoldings(IAccount account, List<IExchangeRate> allExchangeRates, List<ICurrency> allCurrencies, IBinanceDataPool binanceDataPool)
        {
            double exchangeCommission = 0.005;

            Object listLocker = new object();
    
            List<KeyValuePair<ICurrency, Task>> orderTasks = new List<KeyValuePair<ICurrency, Task>>();
            List<KeyValuePair<ICurrency, CancellationToken>> cancellationTokens = new List<KeyValuePair<ICurrency, CancellationToken>>();
            while (true)
            {
                account.RefreshHoldings();

                List<ICurrency> ownedCurrencies = account.Balances.Keys.Except(Currency.CurrenciesToTrade).ToList();
                List<IExchangeRate> ownedExchangeRates = allExchangeRates.Where(exch =>
                    ownedCurrencies.Any(curr => curr == exch.MainCurrency && exch.ReferenceCurrency == Currency.UsDollar)).ToList();

                IDictionary<IExchangeRate, Trend.TrendStatus> statuses = new Dictionary<IExchangeRate, Trend.TrendStatus>();
                foreach (var ownedExchangeRate in ownedExchangeRates)
                {
                    lock (listLocker)
                    {
                        if (orderTasks.Any(orderTask1 => orderTask1.Key == ownedExchangeRate.MainCurrency))
                        {
                            continue;
                        }
                    }

                    ITrend trend = new Trend();
                    Trend.TrendStatus action = trend.GetTrendStatus(
                        ownedExchangeRate, account.InitialBalancesInUsd[ownedExchangeRate.MainCurrency], account.BalancesInUsd[ownedExchangeRate.MainCurrency], 0.01, 0.1);
                    statuses.Add(ownedExchangeRate, action);


                    if (action == Trend.TrendStatus.Keep)
                    {
                        continue;
                    }

                    Task orderTask = null;
                    CancellationToken token = new CancellationToken(false);
                    Order order = new Order(ownedExchangeRate, account.Balances[ownedExchangeRate.MainCurrency] * 0.90, binanceDataPool);
                    switch (action)
                    {
                        case Trend.TrendStatus.Buy:
                        {
                            orderTask = new Task(() =>
                            {
                                order.PlaceBuyOrder();
                            }, token);

                        }
                            break;
                        case Trend.TrendStatus.BuyNow:
                        {
                            orderTask = new Task(() =>
                            {
                                order.PlaceBuyOrder();
                            });
                        }
                            break;
                        case Trend.TrendStatus.SellNow:
                        {
                            orderTask = new Task(() =>
                            {
                                order.PlaceSellOrder();
                            });
                        }
                            break;
                        case Trend.TrendStatus.Sell:
                        {
                            orderTask = new Task(() =>
                            {
                                order.PlaceSellOrder();
                                ;
                            });
                        }
                            break;
                    }

                    if (orderTask == null)
                    {
                        continue;
                    }

                    orderTask.ContinueWith((task, state) =>
                    {
                        lock (listLocker)
                        {
                            orderTasks.RemoveAll(orderTask1 => orderTask1.Key == ownedExchangeRate.MainCurrency);
                            cancellationTokens.RemoveAll(orderTask1 => orderTask1.Key == ownedExchangeRate.MainCurrency);
                        }
                    }, null);

                    lock (listLocker)
                    {
                        orderTasks.Add(new KeyValuePair<ICurrency, Task>(ownedExchangeRate.MainCurrency, orderTask));
                        cancellationTokens.Add(new KeyValuePair<ICurrency, CancellationToken>(ownedExchangeRate.MainCurrency, token));
                    }
                    orderTask.Start();
                }
            }
        }

        
    */
        public static void DoWork(IAccount account, TimeSpan refreshRate, IBinanceDataPool binanceDataPool, bool trade)
        {
            Console.WriteLine("Hi, I provide this software \"as is\" free of charge to the crypto community.");
            Console.WriteLine("However, feel free to send any gratuaty to Ethereum (ETH) 0xcb6803b1afa702c220144cf3828c356e936291f9");
            Console.WriteLine("");

            int rowsOfInitialText = 3;
            double exchangeCommission = 0.005;
            TimeSpan sizeOfTrend = TimeSpan.FromMinutes(1);

            IDictionary<ICurrency, Trend.TrendStatus> trends = account.SupportedCurrencies.ToDictionary
                (currency => (ICurrency)currency, currency => Trend.TrendStatus.NotEnoughData);
  

            Object listLocker = new object();
            List<KeyValuePair<ICurrency, Task>> orderTasks = new List<KeyValuePair<ICurrency, Task>>();
            List<KeyValuePair<ICurrency, CancellationToken>> cancellationTokens = new List<KeyValuePair<ICurrency, CancellationToken>>();
            while (true)
            {
                account.RefreshHoldings();

                List<ICurrency> ownedCurrencies = account.Balances.Keys.ToList();
                IDictionary<ICurrency, double> balancesUsd = account.BalancesInUsd;

                foreach (var balance in balancesUsd)
                {
                    int currencyIndex = balancesUsd.Keys.ToList().IndexOf(balance.Key);

                    IExchangeRate ownedExchangeRate = account.ExchangeRates.First(exch => exch.MainCurrency == balance.Key && exch.ReferenceCurrency == Currency.UsDollar);

                    ITrend trend = new Trend();

                    DateTime now = DateTime.Now;
                    double dataPointHalfMinute = trend.GetChangePercentage(ownedExchangeRate, TimeSpan.FromSeconds(30)) * 100.0;
                    double dataPoint1Minutes = trend.GetChangePercentage(ownedExchangeRate, TimeSpan.FromMinutes(1)) * 100.0;
                    double dataPoint5Minutes = trend.GetChangePercentage(ownedExchangeRate, TimeSpan.FromMinutes(5)) * 100.0;
                    double dataPoin30Minutes = trend.GetChangePercentage(ownedExchangeRate, TimeSpan.FromMinutes(30)) * 100.0;
                    double dataPoin60Minutes = trend.GetChangePercentage(ownedExchangeRate, TimeSpan.FromMinutes(60)) * 100.0;
                    double dataPoinLifeTime = trend.GetChangePercentage(ownedExchangeRate, TimeSpan.FromHours(24)) * 100.0;
                    double currencyBalance = (account.BalancesInUsd[ownedExchangeRate.MainCurrency] - account.InitialBalancesInUsd[ownedExchangeRate.MainCurrency]);
                    double percentageGrowth = trend.GetChangePercentage(ownedExchangeRate) * 100.0;

                    Trend.TrendStatus action = trends[balance.Key] = trend.GetTrendStatus(ownedExchangeRate, TimeSpan.FromMinutes(1),
                        account.InitialBalancesInUsd[ownedExchangeRate.MainCurrency], account.BalancesInUsd[ownedExchangeRate.MainCurrency], 0.01, 0.3);

                    List<WriteLineStruct> writeLineList = new List<WriteLineStruct>()
                    {
                        new WriteLineStruct() {Text = (ownedExchangeRate.MainCurrency.Symbol + " : ${0:0.00}, "), Value = account.BalancesInUsd[ownedExchangeRate.MainCurrency], Threshold = double.NaN},
                        new WriteLineStruct() {Text = "Initial Value : $ : ${0:0.00}, ", Value = account.InitialBalancesInUsd[ownedExchangeRate.MainCurrency], Threshold = double.NaN},
                        new WriteLineStruct() {Text = "Change : ${0:0.00}, ", Value = currencyBalance, Threshold = 1.0},
                        new WriteLineStruct() {Text = "Growth : {0:0.00}%, ", Value = percentageGrowth, Threshold = 0.1},
                        new WriteLineStruct() {Text = "Unit Price : ${0:0.00}, ", Value = ownedExchangeRate.Price, Threshold = double.NaN},
                        new WriteLineStruct() {Text = (action + ", ").PadRight(10), Value = (int)action, Threshold = 0.9 },                       
                        new WriteLineStruct() {Text = !double.IsNaN(dataPointHalfMinute) ? "----30 Seconds Change : {0:0.00}%, " : "", Value = dataPointHalfMinute, Threshold = 1.0},
                        new WriteLineStruct() {Text = !double.IsNaN(dataPoint1Minutes) ?"1 Minute Change : {0:0.00}%, " : "", Value = dataPoint1Minutes, Threshold = 1.0},
                        new WriteLineStruct() {Text = !double.IsNaN(dataPoint5Minutes) ?"5 Minute Change : {0:0.00}%, " : "", Value = dataPoint5Minutes, Threshold = 1.0},
                        new WriteLineStruct() {Text = !double.IsNaN(dataPoin30Minutes) ?"30 Minute Change : {0:0.00}%, " : "", Value = dataPoin30Minutes, Threshold = 1.0},
                        new WriteLineStruct() {Text = !double.IsNaN(dataPoin60Minutes) ?"60 Minute Change : {0:0.00}%, " : "", Value = dataPoin60Minutes, Threshold = 1.0},
                        new WriteLineStruct() {Text = !double.IsNaN(dataPoinLifeTime) ?"Ever Change : {0:0.00}%, " : "", Value = dataPoinLifeTime, Threshold = 1.0},
                    };
                    WriteLine(writeLineList, currencyIndex + rowsOfInitialText);

                    if (!trade)
                    {
                        continue;
                    }

                    lock (listLocker)
                    {
                        if (orderTasks.Any(orderTask1 => orderTask1.Key == ownedExchangeRate.MainCurrency))
                        {
                            continue;
                        }
                    }

                    if (action == Trend.TrendStatus.Keep)
                    {
                        continue;
                    }

                    CancellationToken token = new CancellationToken(false);
                    Order order = new Order(ownedExchangeRate.ExchangeRateSymbol, account.Balances[ownedExchangeRate.MainCurrency], binanceDataPool);

                    Task orderTask = new Task(() =>
                    {
                        switch (action)
                        {
                            case Trend.TrendStatus.BuyNow:
                            {
                                order.PlaceBuyOrder(ownedExchangeRate.Price * 1.01);
                            }
                            break;
                            case Trend.TrendStatus.Buy:
                            {
                                order.PlaceBuyOrder(ownedExchangeRate.Price);
                            }
                            break;
                            case Trend.TrendStatus.SellNow:
                            {
                                order.PlaceSellOrder(ownedExchangeRate.Price);
                                
                            }
                            break;
                            case Trend.TrendStatus.Sell:
                            {
                                order.PlaceSellOrder(ownedExchangeRate.Price * 0.99);
                            }
                            break;

                            
                        }

                    }, token);

                    
                    if (orderTask == null)
                    {
                        continue;
                    }

                    orderTask.ContinueWith((task, state) =>
                    {
                        lock (listLocker)
                        {
                            orderTasks.RemoveAll(orderTask1 => orderTask1.Key == ownedExchangeRate.MainCurrency);
                            cancellationTokens.RemoveAll(orderTask1 => orderTask1.Key == ownedExchangeRate.MainCurrency);
                        }
                    }, null);

                    lock (listLocker)
                    {
                        orderTasks.Add(new KeyValuePair<ICurrency, Task>(ownedExchangeRate.MainCurrency, orderTask));
                        cancellationTokens.Add(new KeyValuePair<ICurrency, CancellationToken>(ownedExchangeRate.MainCurrency, token));
                    }
                    orderTask.Start();
                }

                List<WriteLineStruct> writeLineListEnd = new List<WriteLineStruct>()
                {
                    new WriteLineStruct() {Text = ("Account Total : ${0:0.00}, "), Value = account.AccountTotalInUsd, Threshold = double.NaN},
                    new WriteLineStruct() {Text = "Initial Account Total : ${0:0.00}, ", Value = account.InitialAccountTotalInUsd, Threshold = double.NaN},
                    new WriteLineStruct() {Text = "Change Account Total : ${0:0.00}, ", Value = account.ChangeAccountTotalInUsd, Threshold = 1.0},
                };
                WriteLine(writeLineListEnd, ownedCurrencies.Count + rowsOfInitialText);

                Thread.Sleep(refreshRate);
            };

        }

        public static void WriteLine(List<WriteLineStruct> contents, int cursorY)
        {
            CursorLock.Enter();
            try
            {
                Console.SetCursorPosition(0, cursorY);
                contents.ForEach(content =>
                {
                    Write(content.Text, content.Value, content.Threshold);
                });
            }
            finally
            {
                CursorLock.Exit();
            }
        }

        public static void Write(String text, double value, double treshold)
        {

            lock (CursorLock)
            {
                if (value <(-1 * treshold) && !double.IsNaN(treshold))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else if (value > treshold && !double.IsNaN(treshold))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }

                Console.Write(String.Format(text, value));
                Console.ForegroundColor = ConsoleColor.White;
             }
        }
    }

}