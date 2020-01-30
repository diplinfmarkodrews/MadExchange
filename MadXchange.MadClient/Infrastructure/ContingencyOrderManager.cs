using MadXchange.Exchange.Domain.Models;
using MadXchange.Exchange.Dto;
using MadXchange.Exchange.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MadXchange.MadClient.Infrastructure
{
    public interface IContingencyOrderManager
    {
        //void RemoveContingencyOrdersAborted(OrderEvent o);
        //List<IOrderPostRequest> GetRequestsOnFilledOrder(OrderEvent o);
        void ReplaceClOrdId(string symbol, string[] clOrdIds);
        void ReplaceClOrdIds(string symbol, string[] clOrdIds);
        IOrderPostRequest CreateContingencyMarketOrderRequest(string symbol);
        IOrderPostRequest ShowContingencyRequest(string symbol, string id);
        void AddOrderRequest(string clOrdId, IOrderPostRequest rq);
        void CancelContingencyOrder(string symbol, string clOrdIds);
        void CancelContingencyOrders(string symbol);
        bool HasOrderRequest(string symbol, string clOrdId);
        IOrderPostRequest GetOrderRequest(string symbol, string clOrdId);
        bool HasContingencyOrders(string symbol);
        List<IOrderPostRequest> GetSymbolOrderRequests(string symbol);
    }


    public class ContingencyOrderManager : IContingencyOrderManager
    {
      
        private static Dictionary<string, Dictionary<string, IOrderPostRequest>> SymbolContingencyOrders = new Dictionary<string, Dictionary<string, IOrderPostRequest>>();

      
        private readonly ILogger _Log;
        private readonly string _UserID;
        public ContingencyOrderManager(string userId, ILoggerFactory log)
        {

            _Log = new Logger<ContingencyOrderManager>(log);
            _UserID = userId;

        }

        //public List<IOrderPostRequest> GetRequestsOnFilledOrder(OrderEvent oE)
        //{
        //    List<IOrderPostRequest> clOrderIds = new List<IOrderPostRequest>();
        //    if (HasContingencyOrders(oE.Symbol))
        //    {
        //        for (int i = 0; i < oE.Orders.Count; i += 2)
        //        {
        //            try
        //            {
        //                if (!string.IsNullOrEmpty(oE.Orders[i].ClOrdId))
        //                {
        //                    if (HasOrderRequest(oE.Symbol, oE.Orders[i + 1].ClOrdId))
        //                    {
        //                        clOrderIds.Add(GetOrderRequest(oE.Symbol, oE.Orders[i].ClOrdId));
        //                        continue;
        //                    }
        //                }
        //                if (HasOrderRequest(oE.Symbol, oE.Orders[i + 1].OrderId))
        //                {
        //                    clOrderIds.Add(GetOrderRequest(oE.Symbol, oE.Orders[i + 1].OrderId));
        //                }
        //            }
        //            catch (Exception err) 
        //            {
        //                _Log.LogError(err, $"{_UserID}:Error checking contingency orders on filled event", oE);
        //            }
        //        }

        //    }
        //    return clOrderIds;
        //}
        //public void RemoveContingencyOrdersAborted(OrderEvent oE)
        //{
            
            
        //    if (HasContingencyOrders(oE.Symbol))
        //    {
        //        for (int i = 0; i < oE.Orders.Count; i += 2)
        //        {
        //            try
        //            {
        //                if (oE.Orders[i + 1].IsPostOnly()) // we exclude postonly and bitmex bug abortions
        //                {
        //                    continue;
        //                }
        //                if (!string.IsNullOrEmpty(oE.Orders[i].ClOrdId) && HasOrderRequest(oE.Symbol, oE.Orders[i].ClOrdId))
        //                {
        //                    CancelContingencyOrder(oE.Symbol, oE.Orders[i].ClOrdId);
                            
        //                    continue;
        //                }
        //                if (HasOrderRequest(oE.Orders.FirstOrDefault().Symbol, oE.Orders[i].OrderId))
        //                {
        //                    CancelContingencyOrder(oE.Symbol, oE.Orders[i].OrderId);
        //                    continue;
        //                }
        //            }
        //            catch (Exception err)
        //            {
        //                _Log.LogError(err, $"{_UserID}:Error checking contingency orders on aborted", oE);
        //            }
        //        }
        //    }
            
        //}
        public void ReplaceClOrdIds(string symbol, string[] clOrdIds)
        {
          
            for (int i = 0; i < clOrdIds.Length; i += 2)
            {
                try
                {
                    SymbolContingencyOrders[symbol][clOrdIds[i + 1]] = SymbolContingencyOrders[symbol][clOrdIds[i]];
                    SymbolContingencyOrders[symbol].Remove(clOrdIds[i]);
                }
                catch (Exception err)
                {
                    _Log.LogError(err, $"{_UserID}: Error replacing clOrdIds: {symbol}", clOrdIds);
                }
            }
          
        }


        public void ReplaceClOrdId(string symbol, string[] clOrdIds)
        {
            
            try
            {
                SymbolContingencyOrders[symbol][clOrdIds[1]] = SymbolContingencyOrders[symbol][clOrdIds[0]];
                SymbolContingencyOrders[symbol].Remove(clOrdIds[0]);
            }
            catch (Exception err)
            {
                _Log.LogError(err, $"{_UserID}: Error replacing clOrdId: {symbol}", clOrdIds);
            }


        }
        public IOrderPostRequest CreateContingencyMarketOrderRequest(string symbol)
        {

            var contList = GetSymbolOrderRequests(symbol);
            var camount = contList.Sum(r => r.Quantity);
            //var createRq = new OrderPostRequest()
            //{
            //    Symbol = symbol,
            //    OrdType = OrderType.Market,
            //    Quantity = camount,
            //    Side = contList.FirstOrDefault().Side,
            //    Leverage = contList.FirstOrDefault().Leverage
            //};
            //return createRq;
            return null;
        }


      

        public IOrderPostRequest ShowContingencyRequest(string symbol, string id)
        {
            
            if (SymbolContingencyOrders.ContainsKey(symbol))
            {
                if (SymbolContingencyOrders[symbol].ContainsKey(id))
                {
                    return SymbolContingencyOrders[symbol][id];
                }
            }
            return null;
           
        }

        public void AddOrderRequest(string clOrdId, IOrderPostRequest rq)
        {
           
            if (!SymbolContingencyOrders.ContainsKey(rq.Symbol))
            {
                SymbolContingencyOrders[rq.Symbol] = new Dictionary<string, IOrderPostRequest>();
            }
            
            SymbolContingencyOrders[rq.Symbol][clOrdId] = rq;

        }

        public void CancelContingencyOrder(string symbol, string clOrdIds)
        {
            if (SymbolContingencyOrders.ContainsKey(symbol))
            {

                SymbolContingencyOrders[symbol].Remove(clOrdIds);
            }            
           
        }
        public void CancelContingencyOrders(string symbol)
        {
            
            SymbolContingencyOrders[symbol] = new Dictionary<string, IOrderPostRequest>();
           
        }
        public void CancelContingencyOrders()
        {
           
            SymbolContingencyOrders = new Dictionary<string, Dictionary<string, IOrderPostRequest>>();
            
            
        }

        public bool HasOrderRequest(string symbol, string clOrdId)
        {
           
            if (SymbolContingencyOrders.ContainsKey(symbol))
            {
                if (SymbolContingencyOrders[symbol].ContainsKey(clOrdId))
                {
                    return true;
                }
            }
            return false;
           
        }
      
        /// removes request with the appropriate symbol and clOrdId and returns it
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="clOrdId"></param>
        /// <returns></returns>
        public IOrderPostRequest GetOrderRequest(string symbol, string clOrdId)
        {
           
            _Log.LogTrace($"{_UserID}:fetching contingency order request", SymbolContingencyOrders);
            if (SymbolContingencyOrders.ContainsKey(symbol))
            {
                if (SymbolContingencyOrders[symbol].ContainsKey(clOrdId))
                {
                    var req = SymbolContingencyOrders[symbol][clOrdId];
                    SymbolContingencyOrders[symbol].Remove(clOrdId);                   
                    return req;
                }
            }
            return null;         
        }

        public List<IOrderPostRequest> GetSymbolOrderRequests(string symbol)
        {
            List<IOrderPostRequest> result = new List<IOrderPostRequest>();
           
            if (SymbolContingencyOrders.ContainsKey(symbol))
            {
                result = SymbolContingencyOrders[symbol].Values.ToList();
                SymbolContingencyOrders[symbol] = new Dictionary<string, IOrderPostRequest>();
            }
            return result;
            
        }

        public bool HasContingencyOrders(string symbol)
        {
            
            if (SymbolContingencyOrders.ContainsKey(symbol))
            {
                if (SymbolContingencyOrders[symbol].Count > 0)
                {
                    return true;
                }
            }
            return false;            
        }
    }
}
