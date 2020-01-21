using MadXchange.Exchange.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MadXchange.Exchange.Interfaces
{
    public interface IOrderPutRequest
    {
        Guid AccountId { get; }
        string Symbol { get; set; }
        decimal? Price { get; set; }
        decimal? Amount { get; set; }
        string OrderId { get; set; }
    }

    public interface IOrderPostRequest
    {
        Guid AccountId { get; }
        string Symbol { get; }
        decimal Quantity { get; }
        decimal Leverage { get; }
        OrderSide Side { get; }
    }
    public interface IRestClient : IRestEquity, IRestPosition, IRestOrder,  IIdentity
    {
        
    }
    
    public interface IIdentity 
    {
        
        Task<IEnumerable<IApiKeyInfo>> GetApiKeyAsync(Guid accountId);
    }

    
    public interface IRestEquity 
    {

        Task<IEnumerable<IWalletHistory>> GetWalletHistory(Guid accountId);        
        Task<IEnumerable<IMargin>> GetMarginAsync(Guid accountId);
        Task<IMargin> GetMarginAsync(Guid accountId, string cur);
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
        Task<IInstrument> GetInstruments();
        Task<IOrderBook> GetOrderBook(string symbol);
    
    }
   
}