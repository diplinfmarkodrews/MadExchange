using MadXchange.Common.Messages;
using System;

namespace MadXchange.Exchange.Messages.Commands
{
	public class SetLeverage : ICommand
    {
		public Guid Id { get; } = Guid.NewGuid();
		public Guid AccountID { get; }
		public string Symbol { get; }
		public decimal Leverage { get; }
		public DateTime TimeStamp { get; } = DateTime.UtcNow;
	
		public SetLeverage(Guid accountID, string symbol, decimal leverage)
		{			
			AccountID = accountID;
			Symbol = symbol;
			Leverage = leverage;
			
		}
	}
}
