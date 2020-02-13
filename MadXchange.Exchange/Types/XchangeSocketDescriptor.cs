using MadXchange.Exchange.Domain.Types;
using ServiceStack;
using System;
using System.Collections.Generic;

namespace MadXchange.Exchange.Types
{
    public class XchangeSocketDescriptor : IOptions
    {
        public Xchange Xchange { get; internal set; }
        public string SocketUrl { get; internal set; }
        public string AuthUrl { get; internal set; }        
        public int KeepAliveInterval { get; internal set; }
        public Dictionary<string, Type> TypeDescriptors { get; set; }
        public Dictionary<string, string[]> FieldDescriptors { get; set; }
        public Dictionary<string, string[]> CombinedStrings { get; set; }
        //combine multible values to get response status
        public Dictionary<string, Func<string[], bool>> Accessors { get; set; }
    }
}