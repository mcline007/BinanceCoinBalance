using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinanceAPI;
using Newtonsoft.Json;

namespace BinanceClient
{
    public class BinanceService : IBinanceService
    {
        private readonly IBinanceClient _binanceClient;
        private readonly List<Prices> _currentPriceList;


        public BinanceService(IBinanceClient binanceClient)
        {
            _binanceClient = binanceClient;
            _currentPriceList = ListPrices();

        }

        //Get depth of a symbol
        public async Task<dynamic> GetDepthAsync(string symbol)
        {
            var result = await _binanceClient.GetAsync<dynamic>("v1/depth", "symbol=" + symbol);

            if (result == null)
            {
                throw new NullReferenceException();
            }

            return result;
        }

        //Get latest price of all symbols
        public async Task<dynamic> GetAllPricesAsync()
        {

            var result = await _binanceClient.GetAsync<dynamic>("v1/ticker/allPrices");
            if (result == null)
            {
                throw new NullReferenceException();
            }

            return result;

        }

        public AccountInfo GetAccountInfo()
        {
            var getAccount = GetAccountAsync();
            Task.WaitAll(getAccount);
            dynamic account = getAccount.Result;

            String data = account.ToString();
            return JsonConvert.DeserializeObject<AccountInfo>(data);
        }

        //Get account information
        public async Task<dynamic> GetAccountAsync()
        {
            var result = await _binanceClient.GetSignedAsync<dynamic>("v3/account");
            if (result == null)
            {
                throw new NullReferenceException();
            }

            return result;
        }

        //Get current positions
        public async Task<dynamic> GetAccountPositionsAsync()
        {
            var result = await _binanceClient.GetSignedAsync<dynamic>("v3/account");
            if (result == null)
            {
                throw new NullReferenceException();
            }

            return result;
        }

        //Get list of open orders
        public async Task<dynamic> GetOrdersAsync(string symbol, int limit = 500)
        {
            var result = await _binanceClient.GetSignedAsync<dynamic>("v3/allOrders", "symbol=" + symbol + "&" + "limit=" + limit);
            if (result == null)
            {
                throw new NullReferenceException();
            }

            return result;
        }

        //Get list of trades for account
        public List<TradeInfo> GetTrades(string symbol)
        {
            var getMyTrades = GetTradesAsync(symbol);
            Task.WaitAll(getMyTrades);
            dynamic trades = getMyTrades.Result;

           String data = trades.ToString();
           return JsonConvert.DeserializeObject<List<TradeInfo>>(data);
        }

        //Get list of trades for account
        public async Task<dynamic> GetTradesAsync(string symbol)
        {
            var result = await _binanceClient.GetSignedAsync<dynamic>("v3/myTrades", "symbol=" + symbol);
            if (result == null)
            {
                throw new NullReferenceException();
            }

            return result;
        }

        //Test LIMIT order
        public async Task<dynamic> PlaceTestOrderAsync(string symbol, string side, double quantity, double price)
        {

            var result = await _binanceClient.PostSignedAsync<dynamic>("v3/order/test",
                "symbol=" + symbol + "&" + "side=" + side + "&" + "type=LIMIT" + "&" + "quantity=" + quantity.ToString() + "&" + "price=" + price.ToString() + "&" + "timeInForce=GTC" + "&" + "recvWindow=6000");
            if (result == null)
            {
                throw new NullReferenceException();
            }

            return result;
        }

        public OrderInfo PlaceTestOrder(string symbol, string side, double quantity, double price)
        {
            var getMyTrades = PlaceTestOrderAsync(symbol, side, quantity, price);
            Task.WaitAll(getMyTrades);
            dynamic trades = getMyTrades.Result;

            String data = trades.ToString();
            return JsonConvert.DeserializeObject<OrderInfo>(data);
        }

        //Place a BUY order, defaults to LIMIT if type is not specified
        public async Task<dynamic> PlaceBuyOrderAsync(string symbol, double quantity, double price, string type = "LIMIT")
        {
            var result = await _binanceClient.PostSignedAsync<dynamic>("v3/order",
                "symbol=" + symbol + "&" + "side=BUY" + "&" + "type=" + type + "&" + "timeInForce=GTC" + "&" + "quantity=" + quantity.ToString() + (type == "LIMIT" ? "&" + "price=" + price.ToString() : String.Empty) + "&" + "recvWindow=5000");
            if (result == null)
            {
                throw new NullReferenceException();
            }

            return result;
        }

        public OrderInfo PlaceBuyOrder(string symbol, double quantity, double price, string type = "LIMIT")
        {
            var getMyTrades = PlaceBuyOrderAsync(symbol, quantity, price, type = "LIMIT");
            Task.WaitAll(getMyTrades);
            dynamic trades = getMyTrades.Result;

            String data = trades.ToString();
            return JsonConvert.DeserializeObject<OrderInfo>(data);
        }

        //Place a SELL order, defaults to LIMIT if type is not specified
        public async Task<dynamic> PlaceSellOrderAsync(string symbol, double quantity, double price, string type = "LIMIT")
        {
            var result = await _binanceClient.PostSignedAsync<dynamic>("v3/order",
                "symbol=" + symbol + "&" + "side=SELL" + "&" + "type=" + type + "&" + "quantity=" + quantity.ToString() + "&" + "price=" + price.ToString() + "&" + "timeInForce=GTC" + "&" + "recvWindow=6000");
            if (result == null)
            {
                throw new NullReferenceException();
            }

            return result;
        }

        public OrderInfo PlaceSellOrder(string symbol, double quantity, double price, string type = "LIMIT")
        {
            var getMyTrades = PlaceSellOrderAsync(symbol, quantity, price, type = "LIMIT");
            Task.WaitAll(getMyTrades);
            dynamic trades = getMyTrades.Result;

            String data = trades.ToString();
            return JsonConvert.DeserializeObject<OrderInfo>(data);
        }

        //Check an order's status
        public async Task<dynamic> CheckOrderStatusAsync(string symbol, int orderId)
        {
            var result = await _binanceClient.GetSignedAsync<dynamic>("v3/order", "symbol=" + symbol + "&" + "orderId=" + orderId);
            if (result == null)
            {
                throw new NullReferenceException();
            }

            return result;
        }

        public OrderInfo CheckOrderStatus(string symbol, int orderId)
        {
            var getMyTrades = CheckOrderStatusAsync(symbol, orderId);
            Task.WaitAll(getMyTrades);
            dynamic trades = getMyTrades.Result;

            String data = trades.ToString();
            return JsonConvert.DeserializeObject<OrderInfo>(data);
        }


        //Cancel an order
        public async Task<dynamic> CancelOrderAsync(string symbol, int orderId)
        {
            var result = await _binanceClient.DeleteSignedAsync<dynamic>("v3/order", "symbol=" + symbol + "&" + "orderId=" + orderId);
            if (result == null)
            {
                throw new NullReferenceException();
            }

            return result;
        }

        public bool CancelOrder(string symbol, int orderId)
        {
            var getMyTrades = CancelOrderAsync(symbol, orderId);
            Task.WaitAll(getMyTrades);
            try
            {
                dynamic trades = getMyTrades.Result;
                String data = trades.ToString();
                return true;
            }
            catch (AggregateException e)
            {
                return false;
            }
        }

        //return a List of Price information for consumption
        public List<Prices> ListPrices(dynamic response)
        {
            List<Prices> prices = new List<Prices>();
            prices = JsonConvert.DeserializeObject<List<Prices>>(response.ToString());
            return prices;

        }

        //Overload for ease of use
        public List<Prices> ListPrices()
        {
            List<Prices> prices = new List<Prices>();
            var task = Task.Run(async () => await _binanceClient.GetAsync<dynamic>("v1/ticker/allPrices"));
            dynamic result = task.Result;
            prices = JsonConvert.DeserializeObject<List<Prices>>(result.ToString());
            return prices;

        }

        public double GetPriceOfSymbol(string symbol)
        {
            List<Prices> prices = new List<Prices>();
            var task = Task.Run(async () => await _binanceClient.GetAsync<dynamic>("v1/ticker/allPrices"));
            dynamic result = task.Result;

            prices = ListPrices(result);

            double priceOfSymbol = (from p in prices
                                    where p.Symbol == symbol
                                    select p.Price).First();

            return priceOfSymbol;
        }

    }
}
