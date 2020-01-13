using MadXchange.Exchange.Domain.Models;
using Microsoft.Extensions.Configuration;

namespace MadXchange.Common.Mex.Specification.RestClientSpec
{
    public class RestClientPosition : RestClientExtend
    {

        public RestClientPosition(IConfiguration config, IUserAccount account, IExpiresTimeProvider timeProvider, ISignatureProvider signatureProvider) : base(config, account, signatureProvider, timeProvider) { }


        //public async Task<IEnumerable<WalletHistoryDto>> GetWalletHistoryAsync(CancellationToken token)
        //{
        //    RestRequest req = new RestRequest(new Uri("Wallet/"), Method.GET);
        //    req = SignRequest(req, new KeyValuePair<string, string>[0]);
        //    var rest = await ExecuteTaskAsync<IEnumerable<WalletHistoryDto>>(req);
        //    return rest.Data;
        //}
        ///// <summary>
        ///// requests all available walletbalances of a client
        ///// </summary>
        ///// <returns></returns>
        //public async Task<IEnumerable<MarginDto>> GetMarginAsync(CancellationToken token)
        //{
        //    var req = new RestRequest(new Uri("Margin/"), Method.GET);
        //    req = SignRequest(req, new KeyValuePair<string, string>[0]);
        //    var rest = await ExecuteTaskAsync<IEnumerable<MarginDto>>(req);
        //    return rest.Data;
        //}

        //public async Task<MarginDto> GetMarginAsync(string currency, CancellationToken token)
        //{
        //    var req = new RestRequest(new Uri($"Margin/{currency}"), Method.GET);
        //    req = SignRequest(req, new KeyValuePair<string, string>[1] { new KeyValuePair<string, string>() });
        //    var rest = await ExecuteTaskAsync<MarginDto>(req);
        //    return rest.Data;
        //}
    }

}
