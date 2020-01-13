using System;
using System.Collections.Generic;
using System.Text;

namespace MadXchange.Exchange.Domain.Models
{
    //Margins have to be stored in List => different base currencies
    public interface IMargin 
    {

        string Currency { get; set; }
        decimal? WalletBalance { get; set; }
        DateTime Timestamp { get; set; }
        decimal? MarginBalance { get; set; }
        decimal? AvailableMargin { get; set; }
    }
   
    public class Margin : IMargin
    {
    
        public decimal? MarginBalance { get; set; } 
        public decimal? WalletBalance { get; set; }
        public string Currency { get; set; }
        public DateTime Timestamp { get; set; }
        public decimal? AvailableMargin { get; set; }
        
    }
}
