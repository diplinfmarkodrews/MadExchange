using MadXchange.Exchange.Contracts;
using MadXchange.Exchange.Domain.Models;
using MadXchange.Exchange.Types;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MadXchange.Exchange.Interfaces
{
    public interface IRestClient : IRestEquity, IRestInstrument, IRestOrder, IRestPosition, IRestIdentity { }

    public interface IRestIdentity
    {
        Task<IEnumerable<IApiKeyInfo>> GetApiKeyAsync(Guid accountId);
    }

    public interface IRestEquity
    {
        Task<IEnumerable<IWalletHistory>> GetWalletHistory(Guid accountId);      
    }

    public interface IRestPosition
    {
        Task<IPosition> GetPositionAsync(Guid accountId, string symbol);

        Task<IEnumerable<IPosition>> GetPositionsAsync(Guid accountId);

        Task<IPosition> PositionLeverage(Guid accountId, string symbol, decimal leverage);
    }

    public interface IRestOrder
    {
        Task<IEnumerable<IOrder>> GetOpenOrdersAsync(Guid accountId);

        Task<IEnumerable<IOrder>> GetOpenOrdersAsync(Guid accountId, string symbol);

        Task<IOrder> GetOrderByIDAsync(Guid accountId, string orderID);

        Task<IOrder> CreateOrder(IOrderPostRequest request);

        Task<IEnumerable<IOrder>> CancelAllOrders(Guid accountId, string symbol);

        Task<IEnumerable<IOrder>> CancelAllOrders(Guid accountId);

        Task<IOrder> CancelOrder(Guid accountId, string symbol, string orderID);

        Task<IOrder> ChangeOrder(IOrderPutRequest request);

        Task<IEnumerable<IOrder>> ChangeOrders(IEnumerable<IOrderPutRequest> requests);

        /// <summary>
        /// close position market order, where amount decides direction: negative => sell
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="price"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        Task<IOrder> ClosePosition(Guid accountId, string symbol, decimal? price, int amount);

        Task<IOrder> ClosePosition(Guid accountId, string symbol, OrderSide side);

        //fetches last orders from exchange to
        Task<IEnumerable<IOrder>> QueryLastOrders(Guid accountId, string symbol);
    }

    public interface IRestInstrument
    {
        Task<IInstrument> GetInstrument(string symbol);

        Task<IEnumerable<IInstrument>> GetInstrument();

        //Task<IOrderBook> GetOrderBook(string symbol);
    }
}