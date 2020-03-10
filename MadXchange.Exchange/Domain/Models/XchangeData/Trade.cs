using MadXchange.Exchange.Contracts;
using MadXchange.Exchange.Contracts.XchangeData;
using MadXchange.Exchange.Types;
using System;

namespace MadXchange.Exchange.Domain.Models
{
    public sealed class Trade
    {
        public Xchange Exchange { get; set; }
        public string Symbol { get; set; }
        public OrderSide Side { get; set; }
        public decimal Size { get; set; }
        public decimal Price { get; set; }        
        public string TradeId { get; set; }        
        public DateTime Timestamp { get; set; }

        public static Trade FromModel(TradeDto dto)
            => new Trade() 
            {
                Exchange = dto.Exchange,
                Symbol = dto.Symbol,
                Side = dto.Side,
                Size = dto.Size,
                Price = dto.Price,
                TradeId = dto.TradeId,
                Timestamp = dto.Timestamp
            };
    }
}