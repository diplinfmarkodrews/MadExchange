using MadXchange.Exchange.Domain.Models;
using MadXchange.Exchange.Interfaces;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MadXchange.Exchange.Infrastructure
{
    public abstract class ClientBase  //, IClient
    {
        private readonly IContingencyOrderManager _corderManager;      
        private readonly ILogger _Log;
        private readonly MarginStore _MarginStore;
        private readonly PositionStore _PositionStore;
        private readonly IRestClient _clientRestExec;
        private readonly IClientStore _clientStore;

        public ClientBase(IRestClient client, IClientStore clientStore, IContingencyOrderManager contingencyOrderManager, ILoggerFactory log) 
        {
            _Log = new Logger<ClientBase>(log);
            _corderManager = contingencyOrderManager;
            _clientRestExec = client;
            _clientStore = clientStore;
            _PositionStore = new PositionStore();
            _MarginStore = new MarginStore();

        }
        #region ApiClient BaseInterface
        
        
       
        public async Task<IEnumerable<IApiKeyInfo>> GetApiKeyAsync() 
        {
            return await _clientRestExec.GetApiKeyAsync();                    
        }
        //public async Task<IEnumerable<IOrder>> QueryLastOrders(string symbol) 
        //{
        //    return await _clientRestExec.QueryLastOrders(symbol);
        //}
        public async Task<IPosition> PositionLeverage(string symbol, decimal leverage) 
        {         
            var pos = await _clientStore.GetPositionAsync(symbol);
            //if (pos.Leverage != leverage) 
            //{
            //    var req = await SendPositionLeverage(symbol, leverage);
            //    return req;
            //}
            return null;// pos;
        }
        //checking for placed tp/sl orders to cancel them
        //protected async Task FilledOrderCheckUp(OrderEvent order)
        //{
        //    var openOrders = await GetOpenOrders(order.Symbol);
        //    if (openOrders != null && openOrders.Count > 0)
        //    {
        //        await CheckForOpenSLTPOrdersAndCancel(order.Orders, openOrders);
        //    }
        //}

        //private async Task CheckForOpenSLTPOrdersAndCancel(List<IOrder> filledOrds, List<IOrder> openorders)
        //{
        //    try
        //    {
        //        var filledSLTPOrd = filledOrds.FirstOrDefault(o => o.IsPegPriceOrder());
        //        if (filledSLTPOrd != null)
        //        {
        //            var symbol = filledSLTPOrd.Symbol;
        //            var pegorder = openorders.FirstOrDefault(o => o.IsPegPriceOrder());
        //            if (pegorder != null)
        //            {

        //                await CancelOrder(pegorder.OrderId);
        //            }
        //        }
        //    }
        //    catch (Exception err) 
        //    {
        //        _Log.LogError(err, $"{_User.ApiKey}:Error checking Sl/Tp orders", openorders);
        //    }
        //}
        
                
        
        /// <summary>
        /// needs to go in another hierarchy
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        

        

        public void StoreMargin(string currency)
        {
            StoreMargin(currency);
        }
        public void StoreMargin(IMargin margin)
        {
            StoreMargin(margin);
        }
        
        public void StorePosition(IPosition position)
        {            
            StorePosition(position);
        }
        public void StorePosition(string symbol) 
        {
            
            StorePosition(symbol);
        }
        //public async Task<IEnumerable<IOrder>> CancelAllOrders(string symbol)
        //{
        //    return await _clientRestExec.CancelAllOrders(symbol);
        //}

        //public async Task<IEnumerable<IOrder>> CancelAllOrders()
        //{
        //    return await _clientRestExec.CancelAllOrders();
        //}

        public async Task<IOrder> CancelOrder(string orderID)
        {
            return await CancelOrder(orderID);
        }
        //public async Task<IEnumerable<IOrder>> GetOpenOrdersAsync(string symbol)
        //{
        //    return await _clientRestExec.GetOpenOrdersAsync(symbol);
        //}
        

        //public async Task<IEnumerable<IMargin>> GetMarginAsync()
        //{
        //    return await _clientRestExec.GetMarginAsync();
        //}
        public async Task<IMargin> GetMarginAsync(string cur)
        {
            return await _clientRestExec.GetMarginAsync(cur);
        }
        public async Task<IPosition> GetPositionAsync(string symbol)
        {
            return await _clientRestExec.GetPositionAsync(symbol);
        }

        //public async Task<IEnumerable<IPosition>> GetPositionsAsync()
        //{
        //    return await _clientRestExec.GetPositionsAsync();
        //}

        //public async Task<IEnumerable<IOrder>> GetOpenOrdersAsync()
        //{
        //    return await _clientRestExec.GetOpenOrdersAsync();
        //}



        public async Task<IOrder> GetOrderByIDAsync(string orderID)
        {
            return await _clientRestExec.GetOrderByIDAsync(orderID);
        }

        #endregion
        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~Client()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

       
        //public async Task<IEnumerable<IWalletHistory>> GetWalletHistory()
        //{
        //    return await _clientRestExec.GetWalletHistory();
        //}

       

        public async Task<IOrder> CreateOrder(Interfaces.IOrderPostRequest request, System.Threading.CancellationToken token)
        {
            return await _clientRestExec.CreateOrder(request);
        }

        public async Task<IOrder> ChangeOrder(IOrderPutRequest request)
        {
            return await _clientRestExec.ChangeOrder(request);
        }

        //public async Task<IEnumerable<IOrder>> ChangeOrders(IEnumerable<IOrderPutRequest> requests)
        //{
        //    return await _clientRestExec.ChangeOrders(requests);
        //}

        protected async Task<IPosition> SendPositionLeverage(string symbol, decimal leverage)
        {
            
            return await _clientRestExec.PositionLeverage(symbol, leverage);
        }

        public async Task<IOrder> ClosePosition(string symbol, decimal? price, int amount)
        {
            return await _clientRestExec.ClosePosition(symbol, price, amount);
        }

        //public async Task<IOrder> ClosePosition(string symbol, OrderSide side)
        //{
        //    return await _clientRestExec.ClosePosition(symbol, side);
        //}

        public abstract Task<IOrder> CreateOrder(Interfaces.IOrderPostRequest request);
        public abstract Task<IOrder> CancelOrder(string symbol, string orderID);


        #endregion
    }
}
