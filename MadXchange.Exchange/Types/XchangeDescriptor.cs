using MadXchange.Connector.Services;
using MadXchange.Exchange.Domain.Types;
using Microsoft.Extensions.Configuration;
using ServiceStack;
using System;

namespace MadXchange.Exchange.Types
{
    public class XchangeDescriptor
    {
        public int Id { get; set; }
        public string Name { get; set; }        
        public string BaseUrl { get; set; }
        public EndPoint[] EndPoints { get; set; }
        public XchangeSocketDescriptor SocketDescriptor { get; set; }
        public ObjectDictionary DomainTypes { get; set; }

        internal void ReadDomainTypes(IConfigurationSection configurationSection)
        {
            throw new NotImplementedException();
        }
    }
}