using System;
using System.Collections.Generic;
using System.Linq;

namespace MadXchange.Exchange.Domain.Models.Dto
{
    public class SocketSubscriptionDto
    {
        public Guid Id { get; private set; }

        public string Channel { get; private set; }

        public IEnumerable<string> Args { get; private set; }

        public bool IsActive { get; set; }

        public Type ReturnType { get; set; }

        

       
    }
}