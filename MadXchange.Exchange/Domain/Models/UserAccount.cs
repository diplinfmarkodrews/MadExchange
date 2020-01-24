using Convey.Types;
using System;


namespace MadXchange.Exchange.Domain.Models
{
    public enum Exchanges
    {
        Unknown,
        BitMex,
        ByBit,
        CoinBase,
        Binance,
        Deribit,
        Kraken
    }

    public interface IUserAccount : IIdentifiable<Guid>
    {
        Env Environment { get; }
        string ApiKey { get; }
        string ApiSecret { get; }
        Exchanges Exchange { get; }
        bool IsEnabled(string symbol);
        void SetClientState(ClientState state);
    }
    public class UserAccount : IUserAccount
    {
        public Guid Id { get; }
        public string AccountID { get; set; }
        public Env Environment { get; }
        public string ApiKey { get; }
        public string ApiSecret { get; }
        public Exchanges Exchange { get; }
        public bool IsEnabled(string symbol) { return ClientState == ClientState.Enabled; }
        public void SetClientState(ClientState state)
        {
            ClientState = state;
        }
        public ClientState ClientState { get; internal set; }
        public UserAccount(Env env, Exchanges exchange, string apikey, string apisecret) 
        {
            Environment = env;
            Exchange = exchange;
            ApiKey = apikey;
            ApiSecret = apisecret;
        }
      
        public UserAccount() { }
    }
    public enum Env
    {
        Test,
        Prod
    }
    public enum ClientState
    {
        Enabled = 1,
        Disabled = 2,
        Removed = 3,
        InvalidKey = 4,
        InsufficientFunds = 5,
        Error = 6
    }
}
