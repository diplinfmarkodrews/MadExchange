using System;
using System.Collections.Generic;
using System.Text;

namespace MadXchange.Common.Mex.Specification
{
    public class SocketMessage
    {
        
        public string MessageString { get; set; }
        public long UtcTS { get; } = DateTime.UtcNow.Ticks;


    }
}
