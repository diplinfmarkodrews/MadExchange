using System;
using System.Collections.Generic;
using System.Text;

namespace MadXchange.Exchange.Domain.Types
{
    public enum Env
    {
        Test = 0,
        Prod = 1
    }

    public enum Exchanges
    {
        Unknown = 0,
        ByBit = 1,
        BitMex = 2,
        Ftx = 3,
        Deribit = 4,
        Binance = 5,
        Coinbase = 6
    }
}
