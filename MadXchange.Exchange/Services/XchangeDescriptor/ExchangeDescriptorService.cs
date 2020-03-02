using MadXchange.Connector.Services;
using MadXchange.Exchange.Configuration;
using MadXchange.Exchange.Domain.Models;
using MadXchange.Exchange.Domain.Types;
using MadXchange.Exchange.Types;
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

        XchangeRequestObject RequestDictionary(Xchange exchange, XchangeHttpOperation routeKey, ObjectDictionary paramObjDict = default);

        /// <summary>
        /// public request function
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="routeKey"></param>
        /// <returns></returns>
        string GetPublicEndPointUrl(Xchange exchange, XchangeHttpOperation routeKey);
        XchangeSocketDescriptor GetSocketDescriptor(Xchange exchange);
        string GetSocketConnectionString(Xchange exchange);
        Types.XchangeDescriptor GetXchangeDescriptor(Xchange xchange);
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
        public string GetPublicEndPointUrl(Xchange exchange, XchangeHttpOperation route)
            => $"{GetExchangeEndPoint(exchange, route).Url}";

        private EndPoint GetExchangeEndPoint(Xchange exchange, XchangeHttpOperation routeKey)
            => _exchangeDescriptors[(int)exchange].EndPoints[(int)routeKey];

        public XchangeRequestObject RequestDictionary(Xchange exchange, XchangeHttpOperation routeKey, ObjectDictionary paramObjDict = default)
            => new XchangeRequestObject(_exchangeDescriptors[(int)exchange], GetExchangeEndPoint(exchange, routeKey), paramObjDict);

        public string GetSocketConnectionString(Xchange exchange)
            => _exchangeDescriptors[(int)exchange].SocketDescriptor.SocketUrl;

        public XchangeSocketDescriptor GetSocketDescriptor(Xchange exchange)
            => _exchangeDescriptors[(int)exchange].SocketDescriptor;

        public Types.XchangeDescriptor GetXchangeDescriptor(Xchange xchange)
            => _exchangeDescriptors[(int)xchange];
    }
}