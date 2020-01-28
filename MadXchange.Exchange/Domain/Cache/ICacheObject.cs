using System;
using System.Collections.Generic;
using System.Text;

namespace MadXchange.Exchange.Domain.Cache
{
    public interface ICacheObject
    {
        bool IsValid();
    }
}
