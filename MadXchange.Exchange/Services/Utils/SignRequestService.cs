using MadXchange.Exchange.Domain.Models;
using MadXchange.Exchange.Infrastructure.Stores;
using ServiceStack;
using System;

namespace MadXchange.Exchange.Services.Utils
{
    public interface ISignRequestsService
    {
        void SignRequestObject(Guid accountId, ref XchangeRequestObject formParamString);

        string SignSocketUrl(Guid accountId, string socketUrl);
        string[] CreatSocketAuthString(Guid id);
    }

    public class SignRequestService : ISignRequestsService
    {
        private readonly IExpiresTimeProvider _expiresTimeProvider;
        private readonly ISignatureProvider _signatureProvider;
        private readonly IApiKeySetStore _apiKeySetStore;

        public SignRequestService(IApiKeySetStore apikeysetRepo)
        {
            _expiresTimeProvider = new ExpiresTimeProvider();
            _signatureProvider = new SignatureProvider();
            _apiKeySetStore = apikeysetRepo;
        }

        public SignRequestService(IApiKeySetStore apikeysetRepo, ISignatureProvider signatureProvider, IExpiresTimeProvider timeProvider)
        {
            _expiresTimeProvider = timeProvider;
            _signatureProvider = signatureProvider;
            _apiKeySetStore = apikeysetRepo;
        }
        //Todo rewrite with ObjectDictionary given by the ExchangeDescriptor
        public string SignSocketUrl(Guid accountId, string socketUrl)
        {
            var keyPair = _apiKeySetStore.GetAccount(accountId);
            if (keyPair is null) 
                throw new InvalidOperationException($"account with given key was not found: {accountId}");
            switch (keyPair.Exchange)
            {
                default://bybit is default by now
                    var timeExpires = _expiresTimeProvider.Get(keyPair.Exchange) + 1000;
                    return $"{socketUrl}?{SignUrlByBit(keyPair, timeExpires)}";
                    
            }
        }

        private string SignUrlByBit(ApiKeySet keyPair, long timeExpires)
        {
            var signString = _signatureProvider.CreateSignature(keyPair.ApiSecret, "GET/realtime" + timeExpires);
            return $"api_key={keyPair.ApiKey}&expires={timeExpires}&signature={signString}";
        }

        /// <summary>
        /// parameterString comes in as ObjectDictionary, headers and signature added
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="formParam"></param>
        /// <returns></returns>
        public void SignRequestObject(Guid accountId, ref XchangeRequestObject paramArray)
        {
            var keyPair = _apiKeySetStore.GetAccount(accountId);
            if (keyPair is null) 
                throw new InvalidOperationException($"account with given key was not found: {accountId}");
            switch (keyPair.Exchange)
            {
                default:
                    var timeExpires = _expiresTimeProvider.Get(keyPair.Exchange);
                    SignRequestByBit(keyPair, ref paramArray, timeExpires);
                    return;
            }
        }

        private void SignRequestByBit(ApiKeySet account, ref XchangeRequestObject paramArray, long timeExpires)
        {            
            var signString = paramArray.AddApiKeyHeaders(account.ApiKey, timeExpires);
            string signature = _signatureProvider.CreateSignature(account.ApiSecret, signString); //{"ret_code":10007,"ret_msg":"Login failed, please login again.","ext_code":"","result":null,"ext_info":null,"token":"","time_now":"1580955671.063169"}
            paramArray.AddSignature(signature);
        }

        public string[] CreatSocketAuthString(Guid id)
        {
            var apiKeySet = _apiKeySetStore.GetAccount(id);
            var time = _expiresTimeProvider.Get(apiKeySet.Exchange);
            var sign = _signatureProvider.CreateSignature(apiKeySet.ApiSecret, "GET/realtime" + time);
            return new string[] { apiKeySet.ApiKey, time.ToString(), sign };

        }
    }
}