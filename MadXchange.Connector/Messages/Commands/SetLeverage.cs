using MadXchange.Common.Messages;
using System;

namespace MadXchange.Connector.Messages.Commands
{
	public class SetLeverage : ICommand
    {
		public Guid Id { get; }		
		public int ExchangeId { get; }
		public Guid AccountID { get; }
		public string Symbol { get; }
		public decimal Leverage { get; }
		public DateTime TimeStamp { get; } = DateTime.UtcNow;
	
		public SetLeverage(Guid id, int exchangeId, Guid accountID, string symbol, decimal leverage)
		{
			Id = id.ToString() == string.Empty ? Guid.NewGuid() : id;
			ExchangeId = exchangeId;
			AccountID = accountID;
			Symbol = symbol;
			Leverage = leverage;
			
		}
	}
}
