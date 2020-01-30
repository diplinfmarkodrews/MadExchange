using Convey.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace MadXchange.Exchange.Domain.Models
{
    public interface IApiKeyInfo
    {
        string ApiKey { get; }
        IEnumerable<string> Permissions { get; }
    }

    public class ApiKeySet : IIdentifiable<Guid>
    {
        public Guid Id { get; } = Guid.NewGuid();
        public Exchanges Exchange { get; }
        public string ApiKey { get; }
        public string ApiSecret { get; }
        public Env Env { get; }
        public ApiKeySet(Exchanges exchange, string apiKey, string secret, Env env = Env.Test)
        {

            Exchange = exchange;
            ApiKey = apiKey;
            ApiSecret = secret;
            Env = env;

        }
    }
}
