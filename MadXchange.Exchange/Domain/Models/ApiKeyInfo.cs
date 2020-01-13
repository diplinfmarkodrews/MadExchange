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
}
