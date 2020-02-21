using System;
using System.Collections.Generic;
using System.Reflection;

namespace MadXchange.Exchange.Domain.Types
{
    public interface IEndPoint : IComparable<EndPoint>
    {
    }

    public sealed class EndPoint : IEndPoint
    {
        public string Name { get; set; }
        //endpoint urls are concarnated with baseurl
        public string Method { get; set; }
        public string Url { get; set; }
        public Dictionary<string, Parameter> Parameter { get; set; }
        public string Result { get; set; }
        public Type ReturnType { get; internal set; }
        
        public int CompareTo(EndPoint obj)
        {
            return Name.CompareTo(obj);
        }
    }

    public sealed class Parameter
    {
        public string ExtName { get; set; }
        public string Type { get; set; }
        public bool Required { get; set; }
    }
}