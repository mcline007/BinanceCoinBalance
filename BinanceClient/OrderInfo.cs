using System;
using System.Collections.Generic;
using System.Text;

namespace BinanceClient
{
   public class OrderInfo
    {
       public String symbol { get; set; }
        public String orderId { get; set; }
        public String clientOrderId { get; set; }
        public String transactTime { get; set; }
        public String price { get; set; }
        public String origQty { get; set; }
        public String executedQty { get; set; }
        public String status { get; set; }
        public String timeInForce { get; set; }
        public String type { get; set; }
        public String side { get; set; }
    }
}
