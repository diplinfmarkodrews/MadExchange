using Convey.CQRS.Commands;
using MadXchange.Exchange.Domain.Models;
using System;

namespace MadXchange.Connector.Messages.Commands
{
	public class SetLeverage : ICommand
    {
		public Guid Id { get; }		
		public Exchanges Exchange { get; }
		public Guid AccountID { get; }
		public string Symbol { get; }
		public decimal Leverage { get; }
		public DateTime TimeStamp { get; } = DateTime.UtcNow;
	
		public SetLeverage(Guid id, Exchanges exchange, Guid accountID, string symbol, decimal leverage)
		{
			Id = id == default ? Guid.NewGuid() : id;
			Exchange = exchange;
			AccountID = accountID;
			Symbol = symbol;
			Leverage = leverage;
			
		}
	}
}
