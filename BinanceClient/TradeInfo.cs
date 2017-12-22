using System;
using System.Collections.Generic;
using System.Text;

namespace BinanceClient
{
    public class TradeInfo
    {
        public String id { get; set; }
        public String orderId { get; set; }
        public String price { get; set; }
        public String qty { get; set; }
        public String commission { get; set; }
        public String commissionAsset { get; set; }
        public String time { get; set; }
        public String isBuyer { get; set; }
        public String isMaker { get; set; }
        public String isBestMatch { get; set; }

    }
}
