using MadXchange.Exchange.Domain.Types;
using System;
using System.Collections.Generic;

namespace MadXchange.Exchange.Types
{

    public class ExchangeDescriptor 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SocketUrl { get; set; }
        public string BaseUrl { get; set; }
        public Dictionary<string, EndPoint> EndPoints { get; set; }
                     

        public ExchangeDescriptor()
        {
            EndPoints = new Dictionary<string, EndPoint>();
        }

   
    }
}
