using MadXchange.Exchange.Domain.Models;
using MadXchange.Exchange.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace MadXchange.Exchange.Domain.Cache
{
    public sealed class TradeCacheObject : ICacheObject
    {

        public Guid Id { get; }
        public Xchange Exchange { get; }
        public string Symbol { get; }
        public List<Trade> Trades { get; set; }
        public bool IsValid()
            => true;
    }
}
