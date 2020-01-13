using MadXchange.Common.Types;
using System;


namespace MadXchange.Exchange.Configuration
{
    public class ExchangeDescriptor : IIdentifiable
    {
        public Guid Id { get; }
        public bool IsEnabled { get; private set; }
        public string Name { get; private set; }
        public string WS { get; private set; }
        public string CS { get; private set; }
        public string RouteWallet { get; private set; }
        public string RouteWalletHistory { get; private set; }
        public string RoutePosition { get; private set; }
        public string RouteOrder { get; private set; }
        public string RouteInstrument { get; private set; }
        public string RouteOrderBook { get; private set; }
        

        public ExchangeDescriptor()
        {
            Id = Guid.NewGuid();
        }
    }
}
