using MadXchange.Exchange.Domain.Models;
using MadXchange.Exchange.Domain.Types;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MadXchange.Exchange.Types
{
    public class XchangeDescriptor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string BaseUrl { get; set; }
        public EndPoint[] EndPoints { get; set; }
        public XchangeSocketDescriptor SocketDescriptor { get; set; }
        public Dictionary<string, Type> DomainTypes { get; set; }
        public string TimeStampString { get; internal set; }
        public string ApiKeyString { get; internal set; }
        public string SignString { get; internal set; }

       

    }
}