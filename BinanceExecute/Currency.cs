using System;
using System.Collections.Generic;
using System.Text;

namespace BinanceExecute
{
    public class Currency : ICurrency
    {
        public String Symbol { private set; get; }
        public String Name { private set; get; }

        public Currency(String name, String symbol)
        {
            Symbol = symbol;
            Name = name;
        }


        public static ICurrency CMTcoin = new Currency("CMT Coin", "CMT");
        public static ICurrency Bitcoin =  new Currency("Bitcoin", "BTC");
        public static ICurrency UsDollar = new Currency("US Dollar", "USDT");
        public static ICurrency BinanceCoin = new Currency("Binance Coin", "BNB");
        public static ICurrency Ethereum = new Currency("Ethereum", "ETH");
        public static ICurrency LiteCoin = new Currency("Lite Coin", "LTC");
        public static ICurrency ARNCoin = new Currency("Aeron", "ARN");
        public static ICurrency BCPTCoin = new Currency("BCPT Coin", "BCPT");
        public static ICurrency ADACoin = new Currency("Cardano", "ADA");
        public static ICurrency ICNCoin = new Currency("ICN Coin", "ICN");
        public static ICurrency XVGCoin = new Currency("Verge", "XVG");
        public static ICurrency MTHCoin = new Currency("MTH Coin", "MTH");
        public static ICurrency QSPCoin = new Currency("Quantstamp", "QSP");

        public static ICurrency SNTCoin = new Currency("SNT Coin", "SNT");
        public static ICurrency OAXCoin = new Currency("OAX Coin", "OAX");
        public static ICurrency RCNCoin = new Currency("RCN Coin", "RCN");
        public static ICurrency CTRCoin = new Currency("CTR Coin", "CTR");
        public static ICurrency FUELCoin = new Currency("Etherparty", "FUEL");
        public static ICurrency POECoin = new Currency("Po.et", "POE");
        public static ICurrency BATCoin = new Currency("BAT Coin", "BAT");

        public static ICurrency LSKCoin = new Currency("LSK Coin", "LSK");
        public static ICurrency LENDCoin = new Currency("LEND Coin", "LEND");
        public static ICurrency GVTCoin = new Currency("Genesis", "GVT");
        public static ICurrency DLTCoin = new Currency("Agrello", "DLT");

        public static ICurrency AMBCoin = new Currency("AMB Coin", "AMB");
        public static ICurrency DNTCoin = new Currency("DNT Coin", "DNT");
        public static ICurrency NEOCoin = new Currency("NEO Coin", "NEO");

        public static ICurrency EOSCoin = new Currency("EOS coin", "EOS");
        public static ICurrency IOTACoin = new Currency("MIOTA", "IOTA");
        public static ICurrency REQCoin = new Currency("REQ Coin", "REQ");

        public static ICurrency TRXCoin = new Currency("Tron", "TRX");
        public static ICurrency TRONCoin = new Currency("TRON Coin", "TRON");
        public static ICurrency KNCCoin = new Currency("KNC Coin", "KNC");
        public static ICurrency XrpCoin = new Currency("Ripple", "XRP");
        public static ICurrency VenCoin = new Currency("VeChain", "VEN");
        public static ICurrency XLMStellarCoin = new Currency("XLMStellar  Coin", "XLM");
        public static ICurrency POWRoin = new Currency("PowerLedge", "POWR");
        public static ICurrency CNDCoin = new Currency("Cindicator", "CND");
        public static ICurrency BCXCoin = new Currency("BCX Coin", "BCX");
        public static ICurrency GasCoin = new Currency("NeoGas", "GAS");
        public static ICurrency BitcoinCash = new Currency("BCC Coin", "BCC");
        public static ICurrency MODCoin = new Currency("Modum", "MOD");
        public static ICurrency FUNCoin = new Currency("FUN Coin", "FUN");
        public static ICurrency GXSCoin = new Currency("GXS Coin", "GXS");
        public static ICurrency BTSCoin = new Currency("BTS Coin", "BTS");
        public static ICurrency WABICoin = new Currency("WABI Coin", "WABI");
        public static ICurrency DashICoin = new Currency("DASH Coin", "DASH");
        public static ICurrency STORJCoin = new Currency("STORJ Coin", "STORJ");

        public static List<ICurrency> CurrenciesToTrade = new List<ICurrency>()
        {
            BinanceCoin,
            Ethereum,
            BitcoinCash,
            Bitcoin,
            LiteCoin,
            NEOCoin,
        };
        public static List<ICurrency> SupportedCurrencies = new List<ICurrency>()
        {
            BinanceCoin,
            Ethereum,
            BitcoinCash,
            Bitcoin,
            LiteCoin,
            NEOCoin,
        };
        public static List<ICurrency> AllCurrencies = new List<ICurrency>()
        {
            STORJCoin,
            DashICoin,
            CMTcoin,
            WABICoin,
            BTSCoin,
            GXSCoin,
            Bitcoin,
            UsDollar,
            BinanceCoin,
            FUNCoin,
            XrpCoin,
            POWRoin,
            Ethereum,
            LiteCoin,
            ARNCoin,
            BCPTCoin,
            ICNCoin,
            XVGCoin,
            MTHCoin,
            QSPCoin,
            SNTCoin,
            OAXCoin,
            RCNCoin,
            CTRCoin,
            FUELCoin,
            POECoin,
            BATCoin,
            LSKCoin,
            LENDCoin,
            GVTCoin,
            DLTCoin,
            AMBCoin,
            CNDCoin,
            DNTCoin,
            NEOCoin,
            EOSCoin,
            REQCoin,
            IOTACoin,
            TRXCoin,
            KNCCoin,
            VenCoin,
            XLMStellarCoin,
            ADACoin,
            GasCoin,
            MODCoin,
            /*BitcoinCash/*
            /*BCXCoin,*/
    };
    }
}
