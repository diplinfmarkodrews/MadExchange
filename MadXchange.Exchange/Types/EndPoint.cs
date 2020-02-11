using System;
using System.Collections.Generic;

namespace MadXchange.Exchange.Domain.Types
{
    public interface IEndPoint : IComparable<EndPoint>
    {
    }

    public sealed class EndPoint : IEndPoint
    {
        public string Name { get; set; }

        public string Url { get; set; }
        public Dictionary<string, Parameter> Parameter { get; set; }
        public string Result { get; set; }
        public string SignString { get; internal set; }
        public string TimeStampString { get; internal set; }
        public string ApiKeyString { get; internal set; }

        public int CompareTo(EndPoint obj)
        {
            return Name.CompareTo(obj);
        }
    }

    public sealed class Parameter
    {
        public string ExtName { get; set; }
        public string Type { get; set; }
        public bool IsRequired { get; set; }
    }
}