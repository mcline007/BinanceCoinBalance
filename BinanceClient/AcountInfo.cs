using System;
using System.Collections.Generic;
using System.Text;

namespace BinanceClient
{
    public class AccountInfo
    {
        public class Balance
        {
            public String asset { get; set; }
            public String free { get; set; }
            public String locked { get; set; }
        }
        public String makerCommission { get; set; }
        public String takerCommission { get; set; }
        public String buyerCommission { get; set; }
        public String canTrade { get; set; }
        public String canWithdraw { get; set; }
        public String canDeposit { get; set; }
        public String updateTime { get; set; }
        public List<Balance> balances { get; set; }
    }
}
