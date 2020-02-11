using MadXchange.Exchange.Configuration;
using MadXchange.Exchange.Domain.Models;
using MadXchange.Exchange.Domain.Types;
using ServiceStack;

namespace MadXchange.Exchange.Services.XchangeDescriptor
{
    public interface IXchangeDescriptorService
    {
        public const string SymbolString = "Symbol";
        public const string LeverageString = "Leverage";
        public const string OrderIdString = "OrderId";
        /// <summary>
        /// main function to fetch access points of exchanges
        /// public endpoint only fetch an url. private endpoints however aquire a RequestObject
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="route"></param>
        /// <returns></returns>

        XchangeRequestObject RequestDictionary(Xchange exchange, XchangeOperation routeKey, ObjectDictionary paramObjDict);

        /// <summary>
        /// public request function
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="routeKey"></param>
        /// <returns></returns>
        string GetPublicEndPointUrl(Xchange exchange, XchangeOperation routeKey);

        string GetSocketConnectionString(Xchange exchange);
    }

    /// <summary>
    /// service to fetch appropriate requestobject, should be used as transient service
    /// future enhancements:
    /// fully generic, use of reflection, more endpoint flexibility => read names generate attributes and add to types
    /// </summary>
    public class ExchangeDescriptorService : IXchangeDescriptorService
    {
        private readonly Types.XchangeDescriptor[] _exchangeDescriptors;

        public ExchangeDescriptorService(IXchangeDescriptorConfiguration exchangeDescriptors)
        {
            _exchangeDescriptors = exchangeDescriptors.StoredExchangeDescriptorConfiguration;
        }

        //can both be put together, providing a string dictionary as function layout
        public string GetPublicEndPointUrl(Xchange exchange, XchangeOperation route) => $"{GetExchangeEndPoint(exchange, route).Url}";

        private EndPoint GetExchangeEndPoint(Xchange exchange, XchangeOperation routeKey) => _exchangeDescriptors[(int)exchange].EndPoints[(int)routeKey];

        public XchangeRequestObject RequestDictionary(Xchange exchange, XchangeOperation routeKey, ObjectDictionary paramObjDict)
        {
            var endPoint = GetExchangeEndPoint(exchange, routeKey);
            return new XchangeRequestObject(exchange, endPoint, paramObjDict);
        }

        public string GetSocketConnectionString(Xchange exchange) => _exchangeDescriptors[(int)exchange].SocketUrl;
    }
}