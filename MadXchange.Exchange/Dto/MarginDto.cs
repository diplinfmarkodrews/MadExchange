using MadXchange.Exchange.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MadXchange.Exchange.Dto
{
    public class MarginDto : IMargin
    {
        public Guid AccountId { get; set; }
        public string XchangeAccountId { get; }
        public string Currency { get; set; }
        public decimal? WalletBalance { get; set; }
        public DateTime Timestamp { get; set; }
        public decimal? MarginBalance { get; set; }
        public decimal? AvailableMargin { get; set; }
    }
}
