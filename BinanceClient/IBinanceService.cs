using System.Collections.Generic;
using System.Threading.Tasks;

namespace BinanceClient
{
    public interface IBinanceService
    {
        Task<object> GetDepthAsync(string symbol);
        Task<object> GetAllPricesAsync();
        AccountInfo GetAccountInfo();
        Task<object> GetAccountAsync();
        Task<object> GetAccountPositionsAsync();
        Task<object> GetOrdersAsync(string symbol, int limit = 500);
        List<TradeInfo> GetTrades(string symbol);
        Task<object> GetTradesAsync(string symbol);
        Task<object> PlaceTestOrderAsync(string symbol, string side, double quantity, double price);
        OrderInfo PlaceTestOrder(string symbol, string side, double quantity, double price);
        Task<object> PlaceBuyOrderAsync(string symbol, double quantity, double price, string type = "LIMIT");
        OrderInfo PlaceBuyOrder(string symbol, double quantity, double price, string type = "LIMIT");
        Task<object> PlaceSellOrderAsync(string symbol, double quantity, double price, string type = "LIMIT");
        OrderInfo PlaceSellOrder(string symbol, double quantity, double price, string type = "LIMIT");
        Task<object> CheckOrderStatusAsync(string symbol, int orderId);
        OrderInfo CheckOrderStatus(string symbol, int orderId);
        Task<object> CancelOrderAsync(string symbol, int orderId);
        bool CancelOrder(string symbol, int orderId);
        List<Prices> ListPrices(dynamic response);
        List<Prices> ListPrices();
        double GetPriceOfSymbol(string symbol);
    }
}