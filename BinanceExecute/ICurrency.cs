using System;

namespace BinanceExecute
{
    public interface ICurrency
    {
        String Name { get; }
        String Symbol { get; }
    }
}