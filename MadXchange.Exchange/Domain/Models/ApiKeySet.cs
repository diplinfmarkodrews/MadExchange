using Convey.Types;
using MadXchange.Exchange.Domain.Types;
using System;
using System.Collections.Generic;

namespace MadXchange.Exchange.Domain.Models
{
    public interface IApiKeyInfo
    {
        string ApiKey { get; }
        IEnumerable<string> Permissions { get; }
    }

    public class ApiKeySet : IIdentifiable<Guid>
    {
        public Guid Id { get; }
        public Xchange Exchange { get; }
        public string ApiKey { get; }
        public string ApiSecret { get; }
        public Env Env { get; }

        public ApiKeySet(Guid id, Xchange exchange, string apiKey, string secret, Env env = Env.Test)
        {
            Id = id.ToString() == string.Empty ? Guid.NewGuid() : id;
            Exchange = exchange;
            ApiKey = apiKey;
            ApiSecret = secret;
            Env = env;
        }
    }
}